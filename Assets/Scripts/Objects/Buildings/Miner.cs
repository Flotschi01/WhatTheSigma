using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Objects.Abstracts
{
    public class Miner : MonoBehaviour, ITransports, IInvInteractable
    {
        private Dictionary<int,Item> _inventory = new Dictionary<int,Item>();
        public ITransports Next { get; set; }

        void Start()
        {
            _inventory.Add(0, new Item(ItemType.IronOre));
            _inventory.Add(1, new Item(ItemType.IronOre));
            _inventory.Add(2, new Item(ItemType.CopperOre));
        }
        public void UpdateItems(Item transfer)
        {
            
            Next?.UpdateItems(transfer);
        }

        public Dictionary<int,Item> GetItems()
        {
            return _inventory;
        }

        public void SetItems(Dictionary<int,Item> items)
        {
            _inventory = items;
        }
    }
}