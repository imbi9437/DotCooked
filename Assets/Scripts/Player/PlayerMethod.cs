using System.Collections;
using System.Collections.Generic;
using Interface;
using Photon.Pun;
using StageSceneContents.ContentsObject;
using StageSceneContents.Interactor;
using UnityEngine;

public partial class Player
{
        
    # region Movement Method
    
    private void Move(Vector2 dir)
    {
        mainAnimator.SetBool(IsMove, true);
        mainAnimator.SetBool(IsCook, false);
        mainAnimator.SetFloat(Horizontal, dir.x);
        mainAnimator.SetFloat(Vertical, dir.y);
        
        mainRigidbody.velocity = dir.normalized * calcSpeed;
        curDir = dir;
    }
    
    private void Idle(Vector2 dir)
    {
        mainAnimator.SetBool(IsMove, false);
        mainRigidbody.velocity = Vector2.zero;
        curDir = dir;
    }
    
    private void IsRun(bool isRun)
    {
        this.isRun = isRun;
        calcSpeed = isRun ? defaultSpeed * 2 : defaultSpeed;
    }
    
    #endregion

    #region Interaction Method

    public void Interaction(IInteractable interactable) => interactable.Interact(this);
    
    
    public IInteractable FindInteractable()
    {
        Vector2 center = (Vector2)mainCollider.bounds.center + curDir.normalized * 1.6f;
        Vector2 size = mainCollider.bounds.size.normalized;

        var hitCol = Physics2D.OverlapBox(center, size, 0, interactableLayer);

        if (hitCol == false) return null;
        if (hitCol.TryGetComponent(out IInteractable interactable) == false) return null;
        
        return interactable;
    }
    
    public void Hovering(IInteractable interactable) => interactable.Hovered(this);
    private void Hovering()
    {
        var target = FindInteractable();

        if (target == SelectedInteractable) return;
        if (target == null && SelectedInteractable != null) SelectedInteractable?.UnHovered(this);
        else Hovering(target);
    }

    private void Interaction()
    {
        if (SelectedInteractable == null)
        {
            if (GrabAble == null) return;
            
            if (PhotonNetwork.InRoom)
            {
                photonView.RPC(nameof(ReleaseRPC),RpcTarget.Others,((InteractableObject)GrabAble).GetViewID());
            }
            
            ReleaseObject();
            return;
        }
        
        if (PhotonNetwork.InRoom)
        {
            if (SelectedInteractable is InteractableObject interactableObject)
                photonView.RPC(nameof(InteractionRPC),RpcTarget.Others,interactableObject.GetViewID());
        }

        Interaction(SelectedInteractable);
    }

    [PunRPC]
    public void InteractionRPC(int viewId)
    {
        var target = PhotonView.Find(viewId)?.GetComponent<IInteractable>();
        if (target == null) return;
        Interaction(target);
    }

    [PunRPC]
    public void ReleaseRPC(int viewId)
    {
        if (GrabAble is InteractableObject obj && obj.GetViewID() == viewId)
        {
            ReleaseObject();
        }
    }
    
    public void GrabObject(IGrabAble grabAble)
    {
        if (grabAble == null) return;
        
        bool isPlating = GrabAble is IngredientObject && grabAble is PlateObject;
        isPlating = isPlating || GrabAble is PlateObject && grabAble is IngredientObject;
        
        if (isPlating == false)
        {
            ReleaseVector = Vector2.zero;
            GrabAble?.Release(this);
        }
        
        grabAble.Grab(this);

        if (isPlating == false)
        {
            mainAnimator.SetTrigger(Take);
        }
    }
    
    public void ReleaseObject()
    {
        if (GrabAble == null) return;
        
        ReleaseVector = isRun ? curDir.normalized : Vector2.zero;
        GrabAble.Release(this);
        if (GrabAble == null)
        {
            int id = isRun ? Throw : PutDown;
            mainAnimator.SetTrigger(id);
        }
    }

    public void CookingAction(bool isDoing, int cookType)
    {
        mainAnimator.SetBool(IsCook, isDoing);
        mainAnimator.SetInteger(CookType, cookType);
    }

    private void PlayerLeaveEvent(Photon.Realtime.Player player)
    {
        if (GrabAble is not IngredientObject obj) return;
        if (obj.OwnerNr() != player.ActorNumber) return;

        GrabAble.Release(this);
        mainAnimator.SetTrigger(Emergency);
    }
    
    #endregion
}
