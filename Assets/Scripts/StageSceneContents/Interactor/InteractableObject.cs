using EPOOutline;
using Interface;
using Photon.Pun;
using UnityEngine;

namespace StageSceneContents.Interactor
{
    [RequireComponent(typeof(Collider2D),typeof(Outlinable), typeof(PhotonView))]
    public abstract class InteractableObject : MonoBehaviour, IInteractable
    {
        public string guid;

        protected PhotonView view;
        private Outlinable outline;

        protected Collider2D mainCollider;
        
        protected virtual void Awake()
        {
            outline = GetComponent<Outlinable>();
            mainCollider = GetComponent<Collider2D>();
            view = GetComponent<PhotonView>();
            
            outline.OutlineParameters.Color = Color.green;
            outline.enabled = false;
        }
        
        public virtual void Hovered(IInteractor interactor)
        {
            if (interactor == null) return;
            
            outline.enabled = true;
            
            interactor.SelectedInteractable?.UnHovered(interactor);
            interactor.SelectedInteractable = this;
        }

        public virtual void UnHovered(IInteractor interactor)
        {
            outline.enabled = false;
            interactor.SelectedInteractable = null;
        }

        public abstract void Interact(IInteractor interactor);
        public int GetViewID() => view.ViewID;
        public void SetViewID(int id) => view.ViewID = id;
        public bool IsMine() => view.IsMine;
        public int OwnerNr() => view.ControllerActorNr;
    }
}
