namespace Artifice.Interfaces {
    public interface IInteractable {
        bool CanInteract { get; set; }
        object[] Interact(params object[] parameters);
    } 
}
