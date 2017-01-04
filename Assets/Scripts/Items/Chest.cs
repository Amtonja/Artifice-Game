using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Items {
    /// <summary>
    /// Holds items and gives them to the player once opened
    /// </summary>
    public class Chest : MonoBehaviour, IInteractable, IItemHolder {
        private bool isOpen;
        private Inventory inventory;
        public string[] itemIDs;

        void Start() {
            // Create a new inventory for the chest
            inventory = new Inventory();
            // If the chest has not been opened yet, add the items
            if (CanInteract) {
                if(itemIDs.Length != 0) inventory.Add(itemIDs);
            }
        }

        #region IInteractable
        public bool CanInteract {
            get { return isOpen; }
            set { isOpen = value; }
        }

        /// <summary>
        /// Interaction method for the IInteractable interface
        /// </summary>
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
