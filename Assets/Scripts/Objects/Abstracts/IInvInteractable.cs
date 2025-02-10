using System.Collections.Generic;

namespace Objects.Abstracts
{
    public interface IInvInteractable
    {
        public Dictionary<int,Item> GetItems();
        public void SetItems(Dictionary<int,Item> items);
    }
}