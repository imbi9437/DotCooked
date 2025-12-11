using System;
using DG.Tweening;
using Manager;
using UnityEngine;

namespace StageSceneContents
{
    public class StartState : MonoStageState
    {
        public override StageState state => StageState.Start;

        private void OnEnable()
        {
            EventManager.Instance.OnGetOrder += stageScene.RegisterOrder;
        }

        private void Update()
        {
            stageScene.ReduceTimeLimit(Time.deltaTime);
        }

        private void OnDisable()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnGetOrder -= stageScene.RegisterOrder;
        }
    }
}
