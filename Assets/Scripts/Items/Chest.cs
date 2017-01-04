using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Items {
    public class Chest : MonoBehaviour, IInteractable, IItemHolder {
        private bool isOpen;
        private Inventory inventory;
        public string[] itemIDs;

        void Start() {
            inventory = new Inventory();
            if (CanInteract) {
                if(itemIDs.Length != 0) inventory.Add(itemIDs);
            }
        }

        #region IInteractable
        public bool CanInteract {
            get { return isOpen; }
            set { isOpen = value; }
        }

        public void Interact() {
            if (!CanInteract) return;
            CanInteract = false;
            // TODO play open anim
            // TODO add inventory items to the player's inventory
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
