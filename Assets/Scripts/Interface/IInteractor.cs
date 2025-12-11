namespace Interface
{
    public interface IInteractor
    {
        public IInteractable SelectedInteractable { get; set; }
        public IInteractable FindInteractable();
        public void Interaction(IInteractable interactable);
        public void Hovering(IInteractable interactable);
    }
}