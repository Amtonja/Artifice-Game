using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Items {
    public class Chest : MonoBehaviour, IInteractable, IItemHolder {
        private bool isOpen;
        private Inventory inventory;
        public string[] itemIDs;

        void Start() {
            inventory = new Inventory();
            if (!isOpen) {
                if(itemIDs.Length != 0) inventory.Add(itemIDs);
            }
        }

        #region IInteractable
        public bool CanInteract {
            get { return isOpen; }
            set { isOpen = value; }
        }

        public object[] Interact(params object[] parameters) {
            if (isOpen) return null;
            isOpen = true;
            // TODO play open anim
            return inventory.Items.ToArray();
        }
        #endregion

        #region IItemHolder
        public Inventory GetInventory {
            get { return inventory; }
        }

        public string[] ItemIDs {
            get { return itemIDs; }
        }
        #endregion
    }
}
