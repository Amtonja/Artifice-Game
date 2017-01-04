using System;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Items {
    /// <summary>
    /// Holds a group of items
    /// </summary>
    public class Inventory {
        private List<Item> items;

        public Inventory() {
            items = new List<Item>();
        }

        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(Item item) {
            Item inList = items.Find(i => i.ID.Equals(item.ID));
            // If the item is in the list already, just increment the quantity
            if (inList != null) inList.Quantity += item.Quantity;
            // Otherwise add the item to the list
            else items.Add(item);
        }

        /// <summary>
        /// Adds an item to the list given the id of the item
        /// </summary>
        /// <param name="ID">The ID of the item to add</param>
        public void Add(string ID) {
            string itemID = ID;
            int count = 0;
            // If the ID contains parens, that means there is more than one, and the number following is the quantity
            if (ID.Contains("(")) {
                ID = ID.Replace(" ", string.Empty);
                int startIndex = ID.IndexOf('(');
                itemID = ID.Substring(0, startIndex);
                string countString = string.Empty;
                // Have a failsafe to exit gracefully
                int failsafe = 0;
                while(ID[++startIndex] != ')') {
                    countString += ID[startIndex];
                    failsafe++;
                    if(failsafe >= 1000000) {
                        Debug.LogError("ID: " + ID + " has no closing parens. Adding one item");
                        countString = "1";
                        break;
                    }
                }
                // Parse the number in the parens
                count = int.Parse(countString);
                
            }
            // Find the item from the ItemManager, and if it exists, add it
            Item item = ItemManager.Instance.GetItem(itemID);
            if(item == null) {
                Debug.LogError("Item with ID: " + itemID + " does not exist. Please check the ID");
                return;
            }
            if (count != 0) item.Quantity = count;
            Add(item);
        }

        /// <summary>
        /// Adds a series of items to the inventory
        /// </summary>
        /// <param name="sequence">The list of item IDs to add to the inventory</param>
        public void Add(string[] sequence) {
            for(int i = 0; i < sequence.Length; i++) {
                Add(sequence[i]);
            }

            Debug.Log(ToString());
        }

        /// <summary>
        /// Adds a series of items to the inventory
        /// </summary>
        /// <param name="items">The list of items to add to the inventory</param>
        public void Add(Item[] items) {
            for (int i = 0; i < items.Length; i++) Add(items[i]);
        }

        /// <summary>
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="item"></param>
        public void Remove(Item item) {
            Item inList = items.Find(i => i.ID.Equals(item.ID));
            if(inList != null) {
                if (inList.Quantity > 1) {
                    inList.Quantity = Mathf.Clamp(inList.Quantity - item.Quantity, 1, int.MaxValue);
                } else items.Remove(inList);
            }
        }

        /// <summary>
        /// Override the ToString method to print out all the obkects in an inventory
        /// </summary>
        /// <returns>The list of all items in an inventory</returns>
        public override string ToString() {
            string info = "===============\n";
            for (int i = 0; i < items.Count; i++) {
                info += items[i].Title + " (" + items[i].Quantity + ")";
            }
            info += "===============\n";
            return info;
        }

        /// <summary>
        /// Returns a list of only the specified items
        /// </summary>
        /// <typeparam name="T">The type of item to filter out</typeparam>
        /// <returns>The list of filtered items</returns>
        public List<Item> GetFilteredItems<T>() where T : Item {
            return items.FindAll(i => i.GetType() == typeof(T));
        }

        #region Sorting Functions
        public void SortByName(bool reverse = false) {
            items.Sort(CompareByName);
            if (reverse) items.Reverse();
        }
        public void SortByValue(bool reverse = false) {
            items.Sort(CompareByValue);
            if (reverse) items.Reverse();
        }
        public void SortByRarity(bool reverse = false) {
            items.Sort(CompareByRarity);
            if (reverse) items.Reverse();
        }
        #endregion

        #region Sorting Predicates
        private static int CompareByName(Item first, Item second) {
            return string.Compare(first.Title, second.Title);
        }
        private static int CompareByValue(Item first, Item second) {
            return first.Worth.CompareTo(second.Worth);
        }
        private static int CompareByRarity(Item first, Item second) {
            return ((int)first.Rarity).CompareTo(((int)second.Rarity));
        }
        #endregion

        #region Properties
        public List<Item> Items {
            get { return items; }
            set { items = value; }
        }
        #endregion
    }
}
