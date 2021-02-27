public interface IInteractionTarget
{
    bool CanInteract();
    bool Interact();
    bool CanPropagate();
    bool Propagate();
}
