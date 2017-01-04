namespace Artifice.Interfaces {
    /// <summary>
    /// Interface for anything that the player can interact with
    /// </summary>
    public interface IInteractable {
        /// <summary>
        /// Whether or not the player can currently interact with this object
        /// </summary>
        bool CanInteract { get; set; }
        /// <summary>
        /// The method to call to invoke the interactions
        /// </summary>
        void Interact();
    } 
}
