using System;
using DG.Tweening;
using CustomExtensions;
using Interface;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using StageSceneContents.Interactor;
using UnityEngine;

namespace StageSceneContents.ContentsObject
{
    public class IngredientObject : GrabAbleObject
    {
        private FoodIngredient data;
        private SpriteRenderer mainRenderer;

        public FoodIngredient Data => data;
        
        protected override void Awake()
        {
            base.Awake();
            mainRenderer = GetComponent<SpriteRenderer>();
        }

        public void InitData(FoodIngredient data)
        {
            this.data = data;
            mainRenderer.sprite = data.sprite;
            mainCollider.ResizingCollider2D(data.sprite);
        }

        public override void Grab(IGrabber grabber)
        {
            if (Grabber == grabber) return;
            if (grabber.GrabAble is PlateObject plate)
            {
                plate.AddIngredient(data);
                Destroy(gameObject);
                if (grabber is Player player)
                    player.SelectedInteractable = null;
            }
            else
                base.Grab(grabber);
        }

        public bool CanCook(CookingMethod method)
        {
            return (data.possibleMethod & method) == method;
        }

        public void CompleteCook(CookingMethod method)
        {
            FoodIngredient resultData = DataManager.Instance.GetCookedResult(data, method);
            InitData(resultData);
        }

        private void OnDestroy()
        {
            layerTween.Kill();
        }
    }
}
