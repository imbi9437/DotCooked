using System.Collections.Generic;
using Interface;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using StageSceneContents.Interactor;

namespace StageSceneContents.ContentsObject
{
    public class ServeArea : GrabberObject
    {
        public override void GrabObject(IGrabAble grabAble)
        {
            if (grabAble is not PlateObject plate) return;
            FoodData plateFood = plate.FoodData;
            
            base.GrabObject(plate);
            
            var garbage = plate.gameObject;
            GrabAble = null;
            Destroy(garbage);
            
            EventManager.Instance.OnServeFood?.Invoke(plateFood);
            if (PhotonNetwork.InRoom)
                MultiplayManager.CallRPC(nameof(MultiplayManager.Instance.ServeFood), RpcTarget.Others,plateFood.id);
        }


        public override void Interact(IInteractor interactor)
        {
            if (interactor is not IGrabber grabber) return;
            if (grabber.GrabAble == null && GrabAble == null) return;
            if (grabber.GrabAble is not PlateObject plate) return;
            
            grabber.ReleaseObject();
            GrabObject(plate);
        }
    }
}
