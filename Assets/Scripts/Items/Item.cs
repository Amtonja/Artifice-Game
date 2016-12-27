using Artifice.Interfaces;

namespace Artifice.Items
{
    /// <summary>
    /// Base item class to hold most of the information
    /// </summary>
    public class Item : IXML, IIdentifiable
    {
        public enum RarityLevel { None, Common, Uncommon, Rare, Legendary, Epic, Mythic }

        protected string id;                       // Name of the item
        protected string title;                    // Name of the item
        protected string description;              // Description of the item
        protected int quantity;                    // How many of the item the owner has
        protected RarityLevel rarity;              // Rarity of the item
        protected int worth;                       // Value of the item

        public Item()
        {
            id = "NULL_ID";
            title = "Generic Item";
            quantity = 1;
            description = "This item had problems loading a description.";
            rarity = RarityLevel.Common;
            worth = 0;
        }

        public Item(string _id, string _title, string _description, RarityLevel _rarity, int _worth)
        {
            id = _id;
            title = _title;
            description = _description;
            rarity = _rarity;
            worth = _worth;
            quantity = 1;
        }

        public string ID {
            get { return id; }
            set { id = value; }
        }

        public string Title {
            get { return title; }
            set { title = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        public RarityLevel Rarity {
            get { return rarity; }
            set { rarity = value; }
        }

        public int Worth {
            get { return worth; }
            set { worth = value; }
        }

        public int Quantity {
            get { return quantity; }
            set { quantity = value; }
        }

        public string ResourcesDir {
            get { return "Files/Items"; }
        }
    } 
}
