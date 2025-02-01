using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Objects.Abstracts;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Player
{
    public class InventoryManager : MonoBehaviour
    {
        public float maxInteractDistance;
        private List<Item> _inventory = new List<Item>(81);
        public GameObject inventoryPanel;
        public GameObject externalPanel;

        private List<GameObject> _intItems = new List<GameObject>(81);
        private List<GameObject> _extItems = new List<GameObject>(81);

        private GameObject _activeDAD;
        private int _oldDADindex;
        private GameObject _oldDADPanel;
        
        private GameObject _PrefabItem;
        private Dictionary<ItemType, Sprite> _sprites = new Dictionary<ItemType, Sprite>();
        
        
        void Start()
        {
            foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
            {
                Texture2D texture = Resources.Load<Texture2D>("ItemSprites/" + item.ToString());
                Sprite itemSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                _sprites.Add(item, itemSprite);
            }
            _inventory = new List<Item>(81)
            {
                new Item(ItemType.IronOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.CopperOre),
                new Item(ItemType.CopperOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.IronOre),
                new Item(ItemType.None)
            };
            _PrefabItem = Resources.Load<GameObject>("Item");
        }

        GameObject lastActiveDAD;
        void Update()
        {
            bool start = Input.GetMouseButtonDown(Keys.BMouseLeft);
            bool stop = Input.GetMouseButtonUp(Keys.BMouseLeft);
            if (start||stop)
                //rewrite this with dynamic sized inventories !!!!!
            {
                int indexInvPanel = GetIndexClickedItem(inventoryPanel.GetComponent<RectTransform>());
                int indexExtPanel = GetIndexClickedItem(externalPanel.GetComponent<RectTransform>());

                if (indexInvPanel != -1)
                {
                    _activeDAD = start ? _intItems[indexInvPanel] : null;
                    _oldDADindex = start ? indexInvPanel : _oldDADindex;
                    _oldDADPanel = start ? inventoryPanel : _oldDADPanel;
                }else if(indexExtPanel != -1)
                {
                    _activeDAD = start ? _extItems[indexExtPanel] : null;
                    _oldDADindex = start ? indexExtPanel : _oldDADindex;
                    _oldDADPanel = start ? externalPanel : _oldDADPanel;
                }
            }

            if (_activeDAD) //is not null
            {
                _activeDAD.GetComponent<RectTransform>().position = Input.mousePosition;
            }

            if (!_activeDAD && lastActiveDAD)
            {
                //Handle snapping in place here
                int index = GetIndexClickedItem(inventoryPanel.GetComponent<RectTransform>());
                Debug.Log(index + "old index:" + _oldDADindex);
                lastActiveDAD.GetComponent<RectTransform>().localPosition = GetPositionFromIndex(index, 
                    new Vector2(9,9), inventoryPanel.GetComponent<RectTransform>());
                //Handle managing inventories here
                if (_oldDADPanel == inventoryPanel)
                {
                    InsertItem(_intItems, _intItems[_oldDADindex], index);
                    _intItems[_oldDADindex] = index == _oldDADindex ? _intItems[_oldDADindex] : null;
                    InsertItem(_inventory, _inventory[_oldDADindex], index);
                    _inventory[_oldDADindex] = index == _oldDADindex ? _inventory[_oldDADindex] : null;;
                }
            }

            lastActiveDAD = _activeDAD;
        }
        void InsertItem(IList items, object item, int index)
        {
            try
            {
                items[index] = item;
                return;
            }
            catch
            {
                for (int i = items.Count-1; i < index-1; i++)
                {
                    items.Add(null);
                }
                items.Add(item);
            }

        }
        int GetIndexClickedItem(RectTransform rectTransform)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                Vector2 relativeMousePosition = Input.mousePosition - rectTransform.position;
                return GetIndexFromPosition(relativeMousePosition, new Vector2(9, 9),
                    inventoryPanel.GetComponent<RectTransform>());
            }
            else
            {
                return -1;
            }
        }

        int GetIndexFromPosition(Vector2 relativePosition, Vector2 dimensions, RectTransform panel)//all items are squares
        {
            int itemSize = (int)(panel.rect.width / dimensions.x);
            int rowIndex = (int)relativePosition.y / -itemSize;
            int columnIndex = (int)relativePosition.x / itemSize;
            //Debug.Log("itemsize:" +  itemSize + "\n" + rowIndex + "," + columnIndex);
            return columnIndex + (rowIndex) * (int)dimensions.x;
        }

        Vector2 GetPositionFromIndex(int index, Vector2 dimensions, RectTransform panel)
        {
            Vector2 pos = Vector2.zero;
            int itemSize = (int)(panel.rect.width / dimensions.x);

            pos.x = index * itemSize + (int)(itemSize/2) - (int)(index / dimensions.x) * dimensions.x*itemSize;
            pos.y = (int)(index / dimensions.x) * -itemSize - (int)(itemSize/2);
            return pos;
        }
        
        /// <summary>
        ///  
        /// </summary>
        /// <returns>the script component of the game object you are looking at
        /// when no GameObject is in range/looked at returns null</returns>
        private Component GetInvComponent()
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out hit, maxInteractDistance, LayerMask.GetMask("Buildings")))
            {
                var obj = hit.collider.gameObject.GetComponents(typeof(IInvInteractable));
                if (obj.Length > 0)
                {
                    return obj[0];
                }
            }

            return null;
        }

        public void OnInvOpen()
        {
            if (GetInvComponent() is IInvInteractable externalInv)
            {
                _extItems.Clear();
                _extItems = DisplayInv(externalInv.GetItems(),new Vector2(9, 9), externalPanel);
            }
            else
            {
                _intItems.Clear();
                _intItems = DisplayInv(_inventory, new Vector2(9, 9), inventoryPanel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">the items (abstract) to display</param>
        /// <param name="dimensions">has to be ints, bsp. 9 by 9 itemgrid</param>
        /// <param name="panel">The object that contains the items as children, standard: external</param>
        List<GameObject> DisplayInv(List<Item> items, Vector2 dimensions, GameObject panel)
        {
            List<GameObject> ret = new List<GameObject>();
            foreach (Transform ka in panel.transform)//delete every item
            {
                Destroy(ka.GameObject());
            }
            foreach (Transform ka in externalPanel.transform)//delete every item in external panel(dirty code)
            {
                Destroy(ka.GameObject());
            }
            if (items.Count > (dimensions.x * dimensions.y))
            {
                throw new ArgumentOutOfRangeException();
            }
            for (var index = 0; index < items.Count; index++)   
            {
                GameObject newItem = Instantiate(_PrefabItem, Vector3.zero, Quaternion.identity);
                newItem.transform.SetParent(panel.transform);
                
                newItem.transform.localPosition = GetPositionFromIndex(
                                                    index, dimensions, panel.GetComponent<RectTransform>());
                newItem.GetComponent<Image>().sprite = _sprites[items[index].Type];
                ret.Add(newItem);
            }
            return ret;
        }
    }
}