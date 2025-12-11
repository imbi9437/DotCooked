using System;
using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using DG.Tweening;
using Manager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace StageSceneContents.UI
{
    [Serializable]
    public class RecipePanel
    {
        public Image ingredientIcon;
        public Image cookingMethodIcon;
    }
    public class OrderPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform movementRect;
    
        [SerializeField] private Slider slider;
        [SerializeField] private Image foodIcon;

        [SerializeField] private RecipePanel[] recipePanels;
        
        [SerializeField] private Sprite[] cookingMethodSprites;
        
        private RectTransform canvasRect;
        private Order order;
        
        public Order CurOrder => order;
        
        private void OnEnable()
        {
            canvasRect ??= GetComponentInParent<Canvas>().transform as RectTransform;

            float canvasWidth = canvasRect.rect.width;
            float myCanvasPos = transform.localPosition.x;
            float childLocalPos = canvasWidth * 0.5f - myCanvasPos + movementRect.rect.width * 0.5f;
            
            Vector2 localPos = movementRect.localPosition;
            localPos.x = childLocalPos;
            movementRect.localPosition = localPos;

            movementRect.DOLocalMoveX(movementRect.rect.width * 0.5f, 4f).SetEase(Ease.Linear);
        }

        private void Update()
        {
            slider.value = order.timeLimit - order.currentTime;
        }

        private void OnDestroy()
        {
            movementRect.DOKill();
        }

        public void Init(Order order)
        {
            this.order = order;
            foodIcon.sprite = order.food.sprite;
            slider.maxValue = order.timeLimit;
            slider.value = order.timeLimit;

            for (int i = 0; i < recipePanels.Length; i++)
            {
                recipePanels[i].ingredientIcon.DOFade(0, 0f);
                recipePanels[i].cookingMethodIcon.DOFade(0, 0f);
            }
            
            for (int i = 0; i < order.food.requiredMaterials.Count; i++)
            {
                var requiredMaterial = order.food.requiredMaterials[i];
                
                var cooked = requiredMaterial as CookedIngredient;
                bool isCookedIngredient = cooked != null;
                
                Sprite sprite = isCookedIngredient ? cooked.rawIngredient.sprite : requiredMaterial.sprite;
                
                recipePanels[i].ingredientIcon.sprite = sprite;
                recipePanels[i].ingredientIcon.DOFade(1, 0f);

                if (isCookedIngredient == false) continue;
                
                recipePanels[i].cookingMethodIcon.sprite = cookingMethodSprites.GetItemByFlags((int)cooked.requiredMethod);
                recipePanels[i].cookingMethodIcon.DOFade(1, 0f);
            }
        }

        public void RemovePanel()
        {
            movementRect.DOKill();
            Destroy(gameObject);
        }
    }
}
