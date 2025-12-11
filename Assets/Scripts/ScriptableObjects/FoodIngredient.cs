using System;
using Interface;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    [Flags]
    public enum CookingMethod
    {
        None = 0,
        Boil = 1<<0,
        Fry = 1<<1,
        Grill = 1<<2,
        Chop = 1<<3,
    }
    
    public abstract class FoodIngredient : ScriptableObject
    {
        public string ingredientName;
        public string id;
        public Sprite sprite;
        public CookingMethod possibleMethod;
        
        #if UNITY_EDITOR
        private string prevName;
        private void OnValidate()
        {
            if (prevName == ingredientName) return;
            string path = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(path, $"{GetType().Name}_{ingredientName}");
            prevName = ingredientName;
        }
        #endif
    }
}
