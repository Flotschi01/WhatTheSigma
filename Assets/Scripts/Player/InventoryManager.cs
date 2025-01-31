using System;
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
        private List<Item> _inventory = new List<Item>();
        public GameObject inventoryPanel;
        public GameObject externalPanel;

        private List<GameObject> _intItems = new List<GameObject>();
        private List<GameObject> _extItems = new List<GameObject>();

        
        private GameObject _PrefabItem;
        private Dictionary<ItemType, Sprite> _sprites = new Dictionary<ItemType, Sprite>();

        void Start()
        {
            foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
            {
                Debug.Log(item.ToString());
                Texture2D texture = Resources.Load<Texture2D>("ItemSprites/" + item.ToString());
                Sprite itemSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                _sprites.Add(item, itemSprite);
            }
            _inventory = new List<Item>();
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.CopperOre));
            _inventory.Add(new Item(ItemType.CopperOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.IronOre));
            _inventory.Add(new Item(ItemType.None));
            _PrefabItem = Resources.Load<GameObject>("Item");
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))//rewrite this with dynamic sized inventorys !!!!!
            {
                RectTransform rectTransform = inventoryPanel.GetComponent<RectTransform>();
                RectTransform extRectTransform = externalPanel.GetComponent<RectTransform>();

                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
                {
                    Vector2 relativeMousePosition = Input.mousePosition - rectTransform.position;
                    Debug.Log(GetIndexFromPosition(relativeMousePosition, new Vector2(9, 9),
                        inventoryPanel.GetComponent<RectTransform>()));
                }else if (RectTransformUtility.RectangleContainsScreenPoint(extRectTransform, Input.mousePosition))
                {
                    Vector2 relativeMousePosition = Input.mousePosition - rectTransform.position;

                    Debug.Log(GetIndexFromPosition(relativeMousePosition, new Vector2(9, 9),
                        externalPanel.GetComponent<RectTransform>()));
                }
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
                Vector3 pos = Vector3.zero;
                pos.x = index * 100 + 50 - (int)(index / dimensions.x) * 900;
                pos.y = (int)(index / dimensions.x) * -100 - 50;
                newItem.transform.localPosition = pos;
                newItem.GetComponent<Image>().sprite = _sprites[items[index].Type];
                ret.Add(newItem);
            }
            return ret;
        }
    }
}