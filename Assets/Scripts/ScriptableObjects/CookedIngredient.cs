using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Food/CookedIngredient",fileName = "CookedIngredient_")]
    public class CookedIngredient : FoodIngredient
    {
        public RawIngredient rawIngredient;
        public CookingMethod requiredMethod;
    }
}