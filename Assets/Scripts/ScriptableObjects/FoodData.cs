using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Food/FoodData",fileName = "FoodData_")]
    public class FoodData : ScriptableObject
    {
        public string foodName;
        public string id;
        public int score;
        public Sprite sprite;
        public List<FoodIngredient> requiredMaterials;
    }
}
