using UnityEngine;
using System.Collections.Generic;
namespace Objects.Abstracts
{
    public class Miner : MonoBehaviour, ITransports, IInvInteractable
    {
        private List<Item> _inventory = new List<Item>();
        public ITransports Next { get; set; }
        public void UpdateItems(Item transfer)
        {
            
            Next?.UpdateItems(transfer);
        }

        public List<Item> GetItems()
        {
            return _inventory;
        }

        public void SetItems(List<Item> items)
        {
            _inventory = items;
        }
    }
}