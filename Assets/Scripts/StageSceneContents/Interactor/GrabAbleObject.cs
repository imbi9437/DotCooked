using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Interface;
using Manager;
using Photon.Pun;
using UnityEngine;

namespace StageSceneContents.Interactor
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GrabAbleObject : InteractableObject, IGrabAble, IPunObservable
    {
        protected Rigidbody2D mainRigidbody;
        
        private int originLayer;
        protected TweenerCore<int,int,NoOptions> layerTween;
        
        public IGrabber Grabber { get; set; }
        protected override void Awake()
        {
            base.Awake();
            mainRigidbody = GetComponent<Rigidbody2D>();
            
            originLayer = gameObject.layer;
            
            view.ObservedComponents.Add(this);
        }

        public virtual void Grab(IGrabber grabber)
        {
            if (Grabber == grabber) return;
            
            layerTween.Kill();
            
            gameObject.layer = LayerMask.NameToLayer("IgnorePhysics");
            transform.SetParent(grabber.GrabPivot);
            transform.localPosition = Vector3.zero;
            
            mainRigidbody.velocity = Vector2.zero;
            mainRigidbody.isKinematic = true;

            grabber.GrabAble = this;
            Grabber = grabber;
        }

        public virtual void Release(IGrabber grabber)
        {
            layerTween = DOTween.To(() => gameObject.layer, x => gameObject.layer = x, originLayer, 0f);
            
            transform.SetParent(null);
            
            mainRigidbody.isKinematic = false;
            mainRigidbody.AddForce(grabber.ReleaseVector * 20f, ForceMode2D.Impulse);
            
            grabber.GrabAble = null;
            Grabber = null;
        }

        public override void Interact(IInteractor interactor)
        {
            if (interactor is not IGrabber grabber) return;
            
            if (Grabber == null) grabber.GrabObject(this);
            else if (Grabber != grabber)
            {
                Grabber.ReleaseObject();
                grabber.GrabObject(this);
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                var pos = (Vector3)stream.ReceiveNext();
                if (transform.parent) return;
                transform.position = pos;
            }
        }
    }
}
