using Interface;
using Manager;
using Photon.Pun;
using StageSceneContents.Interactor;
using UnityEngine;

namespace StageSceneContents.ContentsObject
{
    public class GarbageCan : GrabberObject
    {
        public override void GrabObject(IGrabAble grabAble)
        {
            base.GrabObject(grabAble);

            if (GrabPivot.childCount <= 0) return;

            var garbage = GrabPivot.GetChild(0).gameObject;
            garbage.TryGetComponent(out PhotonView photonView);
            GrabAble = null;

            if (PhotonNetwork.InRoom)
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(garbage);   
                }
            }
            else
                Destroy(garbage);
        }


        public override void Interact(IInteractor interactor)
        {
            if (interactor is not IGrabber grabber) return;
            if (grabber.GrabAble == null) return;
            
            IGrabAble interactorGrabAble = null;
            IGrabAble myGrabAble = null;

            if (grabber.GrabAble != null)
                interactorGrabAble = grabber.GrabAble;

            if (GrabAble != null)
                myGrabAble = GrabAble;

            if (interactorGrabAble is PlateObject plate)
            {
                plate.CleanPlate();
                return;
            }
            
            grabber.ReleaseObject();
            GrabObject(interactorGrabAble);
        }

        [PunRPC]
        public void DestroyGarbage(int id)
        {
            var obj = PhotonNetwork.GetPhotonView(id).gameObject;
            PhotonNetwork.Destroy(obj);
        }
    }
}