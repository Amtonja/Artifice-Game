using System;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Items {
    public class Inventory {
        private List<Item> items;

        public Inventory() {
            items = new List<Item>();
        }

        public void Add(Item item) {
            Item inList = items.Find(i => i.ID.Equals(item.ID));
            if (inList != null) inList.Quantity++;
            else items.Add(item);
        }

        public void Add(string ID) {
            string itemID = ID;
            int count = 0;
            if (ID.Contains("(")) {
                ID = ID.Replace(" ", string.Empty);
                int startIndex = ID.IndexOf('(');
                itemID = ID.Substring(0, startIndex);
                string countString = string.Empty;
                while(ID[++startIndex] != ')') {
                    countString += ID[startIndex];
                }
                count = int.Parse(countString);
                
            }
            Item item = ItemManager.Instance.GetItem(itemID);
            if(item == null) {
                Debug.LogError("Item with ID: " + itemID + " does not exist. Please check the ID");
                return;
            }
            if (count != 0) item.Quantity = count;
            Add(item);
        }

        public void Add(string[] sequence) {
            for(int i = 0; i < sequence.Length; i++) {
                Add(sequence[i]);
            }

            PrintObjects();
        }

        public void Add(Item[] items) {
            for (int i = 0; i < items.Length; i++) Add(items[i]);
        }

        public void Remove(Item item) {
            Item inList = items.Find(i => i.ID.Equals(item.ID));
            if(inList != null) {
                if (inList.Quantity > 1) inList.Quantity--;
                else items.Remove(inList);
            }
        }

        public void PrintObjects() {
            Debug.Log("===============");
            for (int i = 0; i < items.Count; i++) {
                Debug.Log(items[i].Title + " (" + items[i].Quantity + ")");
            }
            Debug.Log("===============");
        }

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
