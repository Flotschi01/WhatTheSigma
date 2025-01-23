using System.Collections.Generic;
using UnityEngine;

namespace Objects.Abstracts
{
    public class Belt : MonoBehaviour, ITransports
    {
        private Item _currentItem;
        public ITransports Next { get; set; }
        public void UpdateItems(Item transfer)
        {
            Next?.UpdateItems(_currentItem);
            _currentItem = transfer;
        }
    }
}