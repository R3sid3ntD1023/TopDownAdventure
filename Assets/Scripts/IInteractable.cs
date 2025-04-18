public interface IInteractable
{
    public void Interact(Interactee interactee)
    {
        if (interactee == null)
            return;

        if (CanInteract(interactee))
        {
            OnInteract(interactee);
        }
    }

    protected void OnInteract(Interactee interactee);

    protected bool CanInteract(Interactee interactee) { return true; }
}
