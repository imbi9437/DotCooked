using System;
using Manager;
using Photon.Pun;
using UnityEngine;

namespace StageSceneContents
{
    public class ReadyState : MonoStageState
    {
        public override StageState state => StageState.Ready;
    
        private void OnEnable()
        {
            //Data Initialize
            var currentStageData = DataManager.Instance.GetSelectStageData();

            stageScene.gainMoney = 0;
        
            stageScene.maxTimeLimit = currentStageData.timeLimit;
            stageScene.curTimeLimit = stageScene.maxTimeLimit;

            EventManager.Instance.OnChangeStageScore?.Invoke(stageScene.gainMoney);
            EventManager.Instance.OnChangeTimeLimit?.Invoke(stageScene.curTimeLimit, stageScene.maxTimeLimit);
            
            EventManager.Instance.OnSuccessOrder += stageScene.ServeComplete;
            EventManager.Instance.OnFailedOrder += stageScene.ServeFailed;
            EventManager.Instance.OnServeFood += stageScene.Serving;

            EventManager.Instance.OnMasterClientSwitched += stageScene.EndStateReasonMasterChange;

            if (PhotonNetwork.IsMasterClient || PhotonNetwork.InRoom == false)
            {
                stageScene.scheduledEvent = new ScheduledEvent(stageScene.CreateOrder, null, currentStageData.orderInterval, true);
                Scheduler.RegisterEvent(stageScene.scheduledEvent);
            }
            
            stageScene.ChangeState(StageState.Start);
        }
    }
}
