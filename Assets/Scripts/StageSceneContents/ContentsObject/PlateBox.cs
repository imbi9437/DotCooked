using Interface;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using StageSceneContents.Interactor;
using UnityEngine;

namespace StageSceneContents.ContentsObject
{
    public class PlateBox : GrabberObject
    {
        [SerializeField] private PlateObject plateObject;
        [SerializeField] private int plateCount = 5;

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private SpriteRenderer plateStateRenderer;
        
        protected override void Awake()
        {
            base.Awake();

            EventManager.Instance.OnServeFood += AddPlate;
            plateStateRenderer.sprite = sprites[plateCount > 0 ? 0 : 1];
        }

        public override void Interact(IInteractor interactor)
        {
            if (interactor is not IGrabber grabber) return;

            if (grabber.GrabAble is PlateObject plate)
            {
                grabber.ReleaseObject();
                Destroy(plate.gameObject);
                AddPlate(null);
                return;
            }

            if (plateCount <= 0) return;
            
            var obj = Instantiate(plateObject, transform.position, Quaternion.identity);;
            obj.Interact(interactor);
            plateCount--;
            plateStateRenderer.sprite = sprites[plateCount > 0 ? 0 : 1];
            
            if (PhotonNetwork.InRoom && interactor is Player player && obj.GetViewID() <= 0)
            {
                obj.SetViewID(PhotonNetwork.AllocateViewID(0/*player.photonView.ControllerActorNr*/));
            }
        }

        private void AddPlate(FoodData foodData)
        {
            plateCount++;
            plateStateRenderer.sprite = sprites[plateCount > 0 ? 0 : 1];
        }
    }
}
