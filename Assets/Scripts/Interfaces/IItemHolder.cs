using Artifice.Items;

namespace Artifice.Interfaces {
    interface IItemHolder {
        string[] ItemIDs { get; }
        Inventory GetInventory { get; }
    }
}