using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Objects.Abstracts;
using Unity.VisualScripting;
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
                        _inventory.Add(new Item(ItemType.IronOre));
            _PrefabItem = Resources.Load<GameObject>("Item");
        }
        private void Update()
        {
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
                DisplayInv(externalInv.GetItems());
            }
            else
            {
                DisplayInv(_inventory);
            }
        }
        void DisplayInv(List<Item> items)
        {
            foreach (Transform ka in inventoryPanel.transform)
            {
                Destroy(ka.GameObject());
            }
            for (var index = 0; index < items.Count; index++)
            {
                GameObject newItem = Instantiate(_PrefabItem, Vector3.zero, Quaternion.identity);
                newItem.transform.SetParent(inventoryPanel.transform);
                newItem.transform.localPosition = new Vector3( index * 100 + 50 - (index / 9) * 900,  (index / 9)*-100 - 50, 0);
                newItem.GetComponent<Image>().sprite = _sprites[items[index].Type];
            }
        }
    }
}