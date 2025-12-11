using System;
using Photon.Pun;
using ScriptableObjects;
using StageSceneContents;
using UnityEngine;

namespace Manager
{
    public partial class MultiplayManager
    {
        [PunRPC]
        public void StartStageRPC(string stageId)
        {
            var stageData = DataManager.Instance.FindStageData(stageId);
            DataManager.Instance.SetSelectStageData(stageData);
            SceneController.LoadScene(stageData.stageScene);
        }
        
        [PunRPC]
        public void StartGameRPC()
        {
            SceneController.LoadScene(SceneType.Main);
        }

        [PunRPC]
        public void CreateOrder(string id, string foodId, float timeLimit, string dataTime)
        {
            Order order = new Order();
            order.food = DataManager.Instance.GetFoodData(foodId);
            order.timeLimit = timeLimit;
            order.startTime = Convert.ToDateTime(dataTime);
            
            EventManager.Instance.OnGetOrder?.Invoke(order);
        }

        [PunRPC]
        public void ServeFood(string id)
        {
            FoodData data = null;
            if (id != string.Empty)
                data = DataManager.Instance.GetFoodData(id);
            EventManager.Instance.OnServeFood?.Invoke(data);
        }
    }
}
