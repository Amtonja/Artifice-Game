using Artifice.Items;

namespace Artifice.Interfaces {
    /// <summary>
    /// Interface for anything that can possess items
    /// </summary>
    interface IItemHolder {
        /// <summary>
        /// The list of items to hold initially
        /// </summary>
        string[] ItemIDs { get; }
        /// <summary>
        /// The inventory to contain the items
        /// </summary>
        Inventory GetInventory { get; }
    }
}