using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Objects.Abstracts;
using UnityEngine;
using UnityEngine.UI;

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
                _sprites.Add(item, Resources.Load<Sprite>("Assets/IMG_Textures/InGameMenus" + item.ToString()));
            }
            _inventory = new List<Item>();
            _inventory.Add(new Item(ItemType.Iron));
            _inventory.Add(new Item(ItemType.Iron));
            _inventory.Add(new Item(ItemType.Iron));
            _inventory.Add(new Item(ItemType.Copper));
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
            Vector2 pos = inventoryPanel.GetComponent<RectTransform>().anchoredPosition;
            foreach (Transform ka in inventoryPanel.transform)
            {
                Destroy(ka);
            }
            for (var index = 0; index < items.Count; index++)
            {
                Vector2 itemPos = new Vector2(pos.x + index * 50, pos.y + (index % 9)*50);
                GameObject newItem = Instantiate(_PrefabItem, pos, Quaternion.identity);
                newItem.GetComponent<Image>().sprite = _sprites[items[index].Type];
            }
        }
    }
}