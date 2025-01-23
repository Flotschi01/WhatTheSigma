using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Objects.Abstracts
{
    public class ResourceNode : MonoBehaviour, ITransports
    {

        public ITransports Next { get; set; }
        public ItemType ItemType { get; set; }
        public float Speed { get; set; } //in items per minute
        public bool spawning{get;set;}
        public void UpdateItems(Item transfer)
        {
            Next?.UpdateItems(transfer);
        }

        void Start()
        {
            StartCoroutine(SpawnItems());
        }

        private IEnumerator SpawnItems()
        {
            while (spawning)
            {
                UpdateItems(new Item(ItemType));
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }
}