using System;

namespace Interface
{
    public interface IInteractable
    {
        public void Hovered(IInteractor interactor);
        public void UnHovered(IInteractor interactor);
        public void Interact(IInteractor interactor);
    }
}
