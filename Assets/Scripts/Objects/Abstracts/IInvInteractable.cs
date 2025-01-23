using System.Collections.Generic;

namespace Objects.Abstracts
{
    public interface IInvInteractable : ITransports
    {
        public List<Item> GetItems();
        public void SetItems(List<Item> items);
    }
}