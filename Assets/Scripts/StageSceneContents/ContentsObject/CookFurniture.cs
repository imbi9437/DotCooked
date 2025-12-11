using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Interface;
using Manager;
using ScriptableObjects;
using StageSceneContents.Interactor;
using UnityEngine.UI;

namespace StageSceneContents.ContentsObject
{
    public class CookFurniture : GrabberObject
    {
        public CookingMethod method;
        public int animateIndex;
        public float cookTime;
        public bool isHandling;

        private bool isCompleteCook;
        private float cookProcess;
        private TweenerCore<float,float,FloatOptions> cookTween;

        private Slider processSlider;

        private Player cookingPlayer;

        protected override void Awake()
        {
            base.Awake();
            processSlider = GetComponentInChildren<Slider>();
            processSlider.gameObject.SetActive(false);
        }

        public override void UnHovered(IInteractor interactor)
        {
            base.UnHovered(interactor);

            if (isHandling == false) return;
            if (isCompleteCook) return;
        
            cookProcess = 0;
            processSlider.value = 0f;
            processSlider.gameObject.SetActive(false);
            cookTween.Kill();
            
            if (isHandling) cookingPlayer?.CookingAction(false,animateIndex);
        }

        public override void GrabObject(IGrabAble grabAble)
        {
            if (grabAble is not IngredientObject ingredient) return;
            if (ingredient.CanCook(method) == false) return;
        
            isCompleteCook = false;
            cookProcess = 0f;
            processSlider.value = 0f;
            processSlider.gameObject.SetActive(true);
            base.GrabObject(grabAble);
        }

        public override void Interact(IInteractor interactor)
        {
            if (interactor is not IGrabber grabber) return;
            if (cookTween != null && cookTween.IsActive()) return;
            
            if (GrabAble == null || isCompleteCook) base.Interact(interactor);
            else if (grabber.GrabAble == null && GrabAble is IngredientObject)
            {
                cookingPlayer = interactor as Player;
                if (isHandling) cookingPlayer?.CookingAction(true,animateIndex);
                
                processSlider.gameObject.SetActive(true);
                cookTween = DOTween.To(() => cookProcess, x => cookProcess = x, 1f,cookTime);
                cookTween.onUpdate += () => processSlider.value = cookProcess;
                cookTween.SetEase(Ease.Linear).OnComplete(CompleteCook);
            }
        }
    
        private void CompleteCook()
        {
            isCompleteCook = true;
            var ingredient = GrabAble as IngredientObject;
            ingredient.CompleteCook(method);
            processSlider.gameObject.SetActive(false);
            
            if (isHandling) cookingPlayer?.CookingAction(false,animateIndex);
        }
    }
}
