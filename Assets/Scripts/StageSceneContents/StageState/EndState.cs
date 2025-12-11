using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using UnityEngine;

namespace StageSceneContents
{
    public class EndState : MonoStageState
    {
        public override StageState state => StageState.End;

        private void OnEnable()
        {
            EventManager.Instance.OnSuccessOrder -= stageScene.ServeComplete;
            EventManager.Instance.OnFailedOrder -= stageScene.ServeFailed;
            EventManager.Instance.OnServeFood -= stageScene.Serving;
            
            EventManager.Instance.OnMasterClientSwitched -= stageScene.EndStateReasonMasterChange;

            if (PhotonNetwork.IsMasterClient || PhotonNetwork.InRoom == false)
            {
                if (stageScene.scheduledEvent != null)
                {
                    stageScene.scheduledEvent.isEnd = true;
                }
            }
                
        
            StageData stageData = DataManager.Instance.GetSelectStageData();
            bool isSuccess = stageScene.gainMoney >= stageData.targetScore[0];
            
            //Show Result UI & ADD Change Scene Event
            EventManager.Instance.OnEndStage?.Invoke(stageScene.gainMoney, stageData);
        
            if (isSuccess) DataManager.Instance.SuccessStage(stageData.id,stageScene.gainMoney);
            DataManager.Instance.SetSelectStageData(null);
        }
    }
}