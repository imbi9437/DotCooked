using System.Collections;
using System.Collections.Generic;
using Interface;
using Photon.Pun;
using UnityEngine;

public class TestScript1 : MonoBehaviour, IGrabber, IInteractor, IInteractable
{
    public Transform GrabPivot { get; set; }
    public IGrabAble GrabAble { get; set; }
    public Vector2 ReleaseVector { get; set; }
    public void GrabObject(IGrabAble grabAble)
    {
        throw new System.NotImplementedException();
    }

    public void ReleaseObject()
    {
        throw new System.NotImplementedException();
    }

    public IInteractable SelectedInteractable { get; set; }
    public IInteractable FindInteractable()
    {
        throw new System.NotImplementedException();
    }

    public void Interaction(IInteractable interactable)
    {
        throw new System.NotImplementedException();
    }

    public void Hovering(IInteractable interactable)
    {
        throw new System.NotImplementedException();
    }

    public void Hovered(IInteractor interactor)
    {
        throw new System.NotImplementedException();
    }

    public void UnHovered(IInteractor interactor)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(IInteractor interactor)
    {
        throw new System.NotImplementedException();
    }
}
