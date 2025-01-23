using System.Collections.Generic;
using UnityEngine;

namespace Objects.Abstracts
{
    public interface ITransports
    {
        public ITransports Next { get; set; }
        public void UpdateItems(Item transfer);
    }
}