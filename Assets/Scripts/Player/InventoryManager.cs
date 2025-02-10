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
        private Dictionary<int,Item> _inventory = new(81); //only used in OnInvOpen & OnInvClose
        public GameObject inventoryPanel; //the gameobject of the inv square
        public GameObject externalPanel;  //the gameobject of the inv square
        [CanBeNull] private IInvInteractable m_ExternalInventory;//the inv class script of the target;
        
        private Dictionary<int, GameObject> _intItems = new (81);//GUI Items for dragAndDrop
        private Dictionary<int, GameObject> _extItems = new (81);//GUI Items for dragAndDrop

        private GameObject _activeDAD;//active Drag and Drop
        private int _oldDADindex;//old Drag and Drop item index 
        private GameObject _oldDADPanel; // panel from where the item is being dragged
        
        private GameObject _PrefabItem;//GUI Item Prefab
        private Dictionary<ItemType, Sprite> _TypeToSprites = new();//Table to look up the corresponding sprite
                                                                    // to optimize runtime because resources.load :(

        
        
        void Awake() // i bin a viech
        {
            foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
            {
                Texture2D texture = Resources.Load<Texture2D>("ItemSprites/" + item.ToString());
                Sprite itemSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                _TypeToSprites.Add(item, itemSprite);
            }

            _inventory = new Dictionary<int,Item>(81)
            {
                {0,new Item(ItemType.IronOre)},
                {1,new Item(ItemType.IronOre)},
                {2,new Item(ItemType.IronOre)},
                {3,new Item(ItemType.IronOre)},
                {4,new Item(ItemType.CopperOre)},
                {5,new Item(ItemType.IronOre)},
                {6,new Item(ItemType.IronOre)},
                {7,new Item(ItemType.IronOre)},
                {8,new Item(ItemType.None)},
                {9,new Item(ItemType.CopperOre)},
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
                Debug.Log(indexInvPanel);
                if (indexInvPanel != -1 && (_intItems.ContainsKey(indexInvPanel)||stop))
                {
                    _activeDAD = start ? _intItems[indexInvPanel] : null;
                    _oldDADindex = start ? indexInvPanel : _oldDADindex;
                    _oldDADPanel = start ? inventoryPanel : _oldDADPanel;
                }else if(indexExtPanel != -1 && (_extItems.ContainsKey(indexExtPanel)||stop))
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

            if ((!_activeDAD) && lastActiveDAD)
            {
                //Handle snapping in place here
                
                RectTransform activeRect = inventoryPanel.GetComponent<RectTransform>();
                int index = GetIndexClickedItem(activeRect);
                if (index == -1)
                {
                    activeRect = externalPanel.GetComponent<RectTransform>();
                    index = GetIndexClickedItem(activeRect);
                    if (index == -1)
                    {
                        Debug.Log("outOfInventory");
                        return;
                    }
                }
                while (_intItems.ContainsKey(index) || _extItems.ContainsKey(index))
                {
                    index++;
                }
                Debug.Log(index + "old index:" + _oldDADindex);
                
                lastActiveDAD.transform.SetParent(activeRect.transform);
                lastActiveDAD.GetComponent<RectTransform>().localPosition = GetPositionFromIndex(index, 
                    new Vector2(9,9), activeRect);
                //Handle managing inventories here
                if (_oldDADPanel == inventoryPanel && index != _oldDADindex)
                {
                    _intItems.Add(index, _intItems[_oldDADindex]);
                    _intItems.Remove(_oldDADindex);
                }
                else if (_oldDADPanel != inventoryPanel && index != _oldDADindex)
                {
                    if (_oldDADPanel == inventoryPanel)
                    {
                        _extItems.Add(index, _intItems[_oldDADindex]);
                        _intItems.Remove(_oldDADindex);
                    }
                }
            }

            lastActiveDAD = _activeDAD;//nothing works in production
        }

        /// <summary>
        /// custom method for my dad gui, takes the index of the item at the current mouse position
        /// </summary>
        /// <param name="rectTransform">the panel of the drag and drop action</param>
        /// <returns>-1 when not in rectTransform, normally the index corrosponding to the position</returns>
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
            m_ExternalInventory = GetInvComponent() as IInvInteractable;
            if (m_ExternalInventory != null)
            {
                _extItems.Clear();
                externalPanel.transform.parent.gameObject.SetActive(true);
                _extItems = DisplayInv(m_ExternalInventory.GetItems(),new Vector2(9, 9), externalPanel);
            }
            else
            {
                _intItems.Clear();
                externalPanel.transform.parent.gameObject.SetActive(false);
                _intItems = DisplayInv(_inventory, new Vector2(9, 9), inventoryPanel);
            }
        }

        public void OnInvClose()
        {   
            m_ExternalInventory?.SetItems(getItemsFromGUI(_extItems));
            _inventory = getItemsFromGUI(_intItems);
        }

        private Dictionary<int, Item> getItemsFromGUI(Dictionary<int, GameObject> GUIInventory)
        {
            Dictionary<int, Item> ret = new Dictionary<int, Item>();
            foreach (var item in GUIInventory)
            {
                ret.Add(item.Key, item.Value.GetComponent<AbstractItemHolder>().item);
            }
            return ret;//ich habe noch nicht auf den anderen panel als child hinzugefügt
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">the items (abstract) to display</param>
        /// <param name="dimensions">has to be ints, bsp. 9 by 9 itemgrid</param>
        /// <param name="panel">The object that contains the items as children, standard: external</param>
        Dictionary<int,GameObject> DisplayInv(Dictionary<int, Item> items, Vector2 dimensions, GameObject panel)
        {
            Dictionary<int,GameObject> ret = new ();
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

            foreach (var kv in items)
            {
                GameObject newItem = Instantiate(_PrefabItem, Vector3.zero, Quaternion.identity);
                newItem.transform.SetParent(panel.transform);
                
                newItem.transform.localPosition = GetPositionFromIndex(
                    kv.Key, dimensions, panel.GetComponent<RectTransform>());
                newItem.GetComponent<Image>().sprite = _TypeToSprites[kv.Value.Type];
                newItem.GetComponent<AbstractItemHolder>().item = kv.Value;
                ret.Add(kv.Key, newItem);
            }
            return ret;
        }
    }
}