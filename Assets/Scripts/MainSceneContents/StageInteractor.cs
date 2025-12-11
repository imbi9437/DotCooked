using System;
using Interface;
using Manager;
using ScriptableObjects;
using UnityEngine;

namespace MainSceneContents
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class StageInteractor : MonoBehaviour, IInitializable<StageData>
    {
        [HideInInspector] public StageData stageData;
        public bool IsCompleteInitialize { get; set; }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            
            EventManager.Instance.OnStageInteract?.Invoke(true, this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            if (EventManager.Instance == null) return;
            
            EventManager.Instance.OnStageInteract?.Invoke(false, this);
        }


        public void Initialize(StageData data)
        {
            stageData = data;
            IsCompleteInitialize = true;
        }
    }
}
