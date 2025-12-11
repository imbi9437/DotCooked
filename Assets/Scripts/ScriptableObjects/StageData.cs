using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Stage/StageData",fileName = "StageData_")]
    public class StageData : UnityEngine.ScriptableObject
    {
        public string stageName;
        public string id;
        public List<FoodData> foods;
        public List<int> targetScore;
        public float timeLimit;
        public float orderInterval;
        public int requiredStarCount;
        public int unlockPrice;
        public SceneInfo stageScene;
        public Sprite stageImage;

#if UNITY_EDITOR
        private void Reset()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string name = Path.GetFileNameWithoutExtension(path);
            stageName = name.Replace("StageData_", "");
            targetScore = new List<int>(3);
        }
#endif
    }
}
