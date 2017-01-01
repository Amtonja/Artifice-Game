using UnityEngine;
using System;
using System.Collections.Generic;
using Artifice.Data;

namespace Artifice.Items {
    public class ItemManager : MonoBehaviour {
        private static ItemManager instance;
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

        private void InitializeItems() {
            List<Potion> potions = SaveLoadManager.Instance.ReadXML<Potion>();
            int max = Mathf.Max(potions.Count);

            items = new Dictionary<string, Item>();
            for(int i = 0; i < max; i++) {
                Item item;
                if (i < potions.Count) {
                    item = potions[i];
                    items.Add(item.ID, item);
                }
            }
        }

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
                    //instance.InitializeItems();
                }
                return instance;
            }
        }
    }
}
