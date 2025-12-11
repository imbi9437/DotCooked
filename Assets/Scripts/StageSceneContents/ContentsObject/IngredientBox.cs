using Interface;
using Photon.Pun;
using ScriptableObjects;
using StageSceneContents.Interactor;
using UnityEngine;

namespace StageSceneContents.ContentsObject
{
    public class IngredientBox : InteractableObject
    {
        public RawIngredient data;
        public IngredientObject ingredientObject;
        public SpriteRenderer iconRenderer;
        
        protected override void Awake()
        {
            base.Awake();
            iconRenderer.sprite = data.sprite;
        }
        
        public override void Interact(IInteractor interactor)
        {
            var obj = Instantiate(ingredientObject, transform.position, Quaternion.identity);
            obj.InitData(data);
            obj.Interact(interactor);

            if (PhotonNetwork.InRoom && interactor is Player player && obj.GetViewID() <= 0)
            {
                obj.SetViewID(PhotonNetwork.AllocateViewID(player.photonView.ControllerActorNr));
            }
        }
    }
}
