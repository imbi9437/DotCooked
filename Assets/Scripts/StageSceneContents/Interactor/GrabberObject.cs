using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using Manager;
using StageSceneContents.ContentsObject;
using StageSceneContents.Interactor;
using UnityEngine;

namespace StageSceneContents.Interactor
{
    public class GrabberObject : InteractableObject, IGrabber
    {
        public Transform GrabPivot { get; set; }
        public IGrabAble GrabAble { get; set; }
        public Vector2 ReleaseVector { get; set; }

        protected override void Awake()
        {
            base.Awake();
            GrabPivot = transform.Find("GrabPivot") ?? transform;
        }

        public virtual void GrabObject(IGrabAble grabAble) => grabAble?.Grab(this);

        public virtual void ReleaseObject() => GrabAble?.Release(this);
        
        public override void Interact(IInteractor interactor)
        {
            if (interactor is not IGrabber grabber) return;
            if (grabber.GrabAble == null && GrabAble == null) return;

            IGrabAble interactorGrabAble = null;
            IGrabAble myGrabAble = null;

            if (grabber.GrabAble != null)
                interactorGrabAble = grabber.GrabAble;

            if (GrabAble != null)
                myGrabAble = GrabAble;
            
            grabber.ReleaseObject();
            ReleaseObject();
            
            GrabObject(interactorGrabAble);
            grabber.GrabObject(myGrabAble);
        }
        
    }
}