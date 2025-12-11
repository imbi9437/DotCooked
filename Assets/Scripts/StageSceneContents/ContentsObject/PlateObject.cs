using System.Collections.Generic;
using Interface;
using Manager;
using ScriptableObjects;
using StageSceneContents.Interactor;
using UnityEngine;

namespace StageSceneContents.ContentsObject
{
    public class PlateObject : GrabAbleObject
    {
        private FoodData foodData;
        private SpriteRenderer foodRenderer;
        private List<FoodIngredient> plateIngredients;
        
        public FoodData FoodData => foodData;

        protected override void Awake()
        {
            base.Awake();
            plateIngredients = new List<FoodIngredient>();
            foodRenderer = transform.Find("FoodRenderer")?.GetComponent<SpriteRenderer>();
        }

        public override void Grab(IGrabber grabber)
        {
            if (grabber is GarbageCan) CleanPlate();
            else if (grabber.GrabAble is IngredientObject ingredient)
            {
                AddIngredient(ingredient.Data);
                
                grabber.GrabAble = null;
                Destroy(ingredient.gameObject);
            }
            
            base.Grab(grabber);
        }

        private void GetFoodData()
        {
            bool isCompleteCook = DataManager.Instance.CheckCompleteCook(plateIngredients, out foodData);
            
            if (isCompleteCook) foodRenderer.sprite = foodData.sprite;
            else foodRenderer.sprite = plateIngredients[0]?.sprite;
        }

        public void CleanPlate()
        {
            foodData = null;
            plateIngredients.Clear();
            foodRenderer.sprite = null;
        }

        public void AddIngredient(FoodIngredient ingredient)
        {
            plateIngredients.Add(ingredient);
            GetFoodData();
        }
    }
}
