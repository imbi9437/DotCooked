using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace StageSceneContents
{
    [Serializable]
    public class Order
    {
        public string id;
        public FoodData food;
        public float timeLimit;
        public float currentTime;
        public DateTime startTime;
    }
}
