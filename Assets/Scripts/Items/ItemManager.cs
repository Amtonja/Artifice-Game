using UnityEngine;
using System;
using System.Collections.Generic;
using Artifice.Data;

namespace Artifice.Items {
    /// <summary>
    /// Manages all the items in the game
    /// </summary>
    public class ItemManager : MonoBehaviour {
        private static ItemManager instance;
        // Will be used mostly for filtering
        public static Type[] ITEM_TYPES = new Type[] { typeof(Item), typeof(Potion) };

        private Dictionary<string, Item> items;

        void Awake() {
            if (instance == null) {   
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
                return;
            }

            InitializeItems();
        }

        /// <summary>
        /// Initializes the item manager
        /// </summary>
        private void InitializeItems() {
            // Get all the items of each type
            List<Item> loadItems = SaveLoadManager.Instance.ReadXML<Item>();
            List<Potion> loadPotions = SaveLoadManager.Instance.ReadXML<Potion>();
            // Grab the longest list count
            int max = Mathf.Max(loadItems.Count, loadPotions.Count);

            items = new Dictionary<string, Item>();
            // Go through the lists, adding all the items tp the dictionary
            for(int i = 0; i < max; i++) {
                Item item;
                if(i < items.Count) {
                    item = loadItems[i];
                    items.Add(item.ID, item);
                }
                if (i < loadPotions.Count) {
                    item = loadPotions[i];
                    items.Add(item.ID, item);
                }
            }
        }

        /// <summary>
        /// Gets an item from the master list
        /// </summary>
        /// <param name="ID">The id of the item to return</param>
        /// <returns>The item from the master list</returns>
        public Item GetItem(string ID) {
            if (!items.ContainsKey(ID)) {
                Debug.LogError("Item with ID: \"" + ID + "\" does not exist. Please check the ID");
                return new Item();
            }
            return items[ID];
        }

        public static ItemManager Instance {
            get {
                if(instance == null) {
                    instance =  new GameObject("Item Manager").AddComponent<ItemManager>();
                }
                return instance;
            }
        }
    }
}
