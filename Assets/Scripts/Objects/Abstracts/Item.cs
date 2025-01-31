using Unity.VisualScripting;

namespace Objects.Abstracts
{
    public enum ItemType
    {
        None,
        IronOre,
        CopperOre
    }
    public class Item
    {
        public float Temp { get; set; }
        public ItemType Type { get; set; }

        public Item(ItemType type)
        {
            Type = type;
            Temp = 20;
        }
        public static Item None
        {
            get
            {
                return new Item(ItemType.None);
            }
            set
            {
                
            }
        }
    }
}