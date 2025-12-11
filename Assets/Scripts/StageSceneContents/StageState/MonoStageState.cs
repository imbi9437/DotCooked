using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StageSceneContents
{
    public abstract class MonoStageState : MonoBehaviour
    {
        public abstract StageState state { get; }
        protected StageScene stageScene;
        
        public void Initialize(StageScene stageScene)
        {
            this.stageScene = stageScene;
        }
    }
}
