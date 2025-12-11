using System;
using System.Collections.Generic;
using DG.Tweening;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using StageSceneContents.Interactor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace StageSceneContents
{
    public enum StageState
    {
        Ready,
        Start,
        End,
    }
    
    public class StageScene : MonoBehaviour
    {
        private Dictionary<StageState, MonoStageState> stateDic;
        private MonoStageState currentState;

        private List<Order> orders = new List<Order>();
        
        public int gainMoney;

        public float maxTimeLimit;
        public float curTimeLimit;
        
        public ScheduledEvent scheduledEvent;

        public List<Transform> spawnPoint;
        public Transform furnitureParent;
        
        private void Awake()
        {
            stateDic = new Dictionary<StageState, MonoStageState>();
            
            var stageStates = GetComponentsInChildren<MonoStageState>(true);
            foreach (var stageState in stageStates)
            {
                stateDic.Add(stageState.state, stageState);
                stageState.gameObject.SetActive(false);
                stageState.Initialize(this);
            }
            
            if (PhotonNetwork.InRoom == false)
                GameManager.Instance.CreatePlayer(spawnPoint[0].position, true, 0);
            else
            {
                foreach (var kvp in PhotonNetwork.CurrentRoom.Players)
                {
                    bool isMine = kvp.Value.IsLocal;
                    int id = (int)kvp.Value.CustomProperties["CharID"];
                    int index = kvp.Key % spawnPoint.Count;
                    GameManager.Instance.CreatePlayer(spawnPoint[index].position, isMine, id);
                }
            }
        }
        private void Start()
        {
            ChangeState(StageState.Ready);
        }

        public void ChangeState(StageState state)
        {
            if (currentState == false)
            {
                currentState = stateDic[state];
                currentState.gameObject.SetActive(true);
                EventManager.Instance.OnStartStageState?.Invoke(state);
                return;
            }
            
            if (currentState.state == state) return;
            
            EventManager.Instance.OnEndStageState?.Invoke(currentState.state);
            currentState.gameObject.SetActive(false);
            
            currentState = stateDic[state];
            EventManager.Instance.OnStartStageState?.Invoke(currentState.state);
            currentState.gameObject.SetActive(true);
        }

        public void ReduceTimeLimit(float value)
        {
            curTimeLimit -= value;
            EventManager.Instance.OnChangeTimeLimit?.Invoke(curTimeLimit, maxTimeLimit);
            
            Order targetOrder = null;
            foreach (var order in orders)
            {
                order.currentTime += value;
                if (order.currentTime < order.timeLimit) continue;
                targetOrder = order;
            }
            
            EventManager.Instance.OnFailedOrder?.Invoke(targetOrder);
            
            if (curTimeLimit <= 0) ChangeState(StageState.End);
        }

        public void CreateOrder(object param)
        {
            Order order = DataManager.Instance.CreateOrder();

            if (PhotonNetwork.IsMasterClient)
            {
                string guid = order.id;
                string id = order.food.id;
                float timeLimit = order.timeLimit;
                string start = order.startTime.ToString("T");
                MultiplayManager.CallRPC(nameof(MultiplayManager.CreateOrder),RpcTarget.OthersBuffered,guid, id,timeLimit,start);
            }
            
            EventManager.Instance.OnGetOrder?.Invoke(order);
        }

        public void RegisterOrder(Order order) => orders.Add(order);
        
        public void Serving(FoodData servingFood)
        {
            if (servingFood == null) return;
            
            Order targetOrder = null;
            float minTime = float.MaxValue;
            bool isSuccess = false;
            
            foreach (var order in orders)
            {
                if (order.currentTime >= order.timeLimit) continue;
                if (order.food != servingFood) continue;
                
                float leftTime = order.timeLimit - order.currentTime;
                if (leftTime >= minTime) continue;
                
                targetOrder = order;
                minTime = leftTime;
                isSuccess = true;
            }
            
            if (isSuccess) EventManager.Instance.OnSuccessOrder?.Invoke(targetOrder);
        }
        
        public void ServeComplete(Order order)
        {
            orders.Remove(order);
            
            gainMoney += order.food.score;
            EventManager.Instance.OnChangeStageScore?.Invoke(gainMoney);
        }

        public void ServeFailed(Order order)
        {
            orders.Remove(order);
        }

        public void EndStateReasonMasterChange(Photon.Realtime.Player masterPlayer)
        {
            ChangeState(StageState.End);
        }
    }
}
