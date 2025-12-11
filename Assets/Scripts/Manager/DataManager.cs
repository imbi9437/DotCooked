using System;
using System.Collections.Generic;
using CustomExtensions;
using Generic;
using ScriptableObjects;
using StageSceneContents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private UserData userData;
        [SerializeField] private List<StageData> stageData;

        [SerializeField] private List<FoodIngredient> ingredients;
        [SerializeField] private List<FoodData> foodData;
        
        
        private StageData selectStageData;
        private List<RawIngredient> rawIngredients;
        private List<CookedIngredient> cookedIngredients;

        private Dictionary<FoodIngredient, int> countMap = new Dictionary<FoodIngredient, int>();
        
        private void OnDestroy()
        {
            if (EventManager.Instance == false) return;
            EventManager.Instance.OnLoadUserData -= OnLoadUserData;
        }

        private void Start()
        {
            rawIngredients = new List<RawIngredient>();
            cookedIngredients = new List<CookedIngredient>();
            
            foreach (FoodIngredient ingredient in ingredients)
            {
                if (ingredient is RawIngredient rawIngredient) rawIngredients.Add(rawIngredient);
                else if (ingredient is CookedIngredient cookedIngredient) cookedIngredients.Add(cookedIngredient);
            }
        }

        private void OnLoadUserData(UserData data)
        {
            userData = data;
        }

        public override void Initialize()
        {
            EventManager.Instance.OnLoadUserData += OnLoadUserData;
            SaveLoad.LoadUserData();
            
            base.Initialize();
        }

        public bool TryUnlockStage(string stageId)
        {
            StageData selectData = stageData.Find(s => s.id == stageId);

            if (selectData == false) return false;
            if (CheckStageUnLocked(stageId)) return false;
            if (userData.starCount < selectData.requiredStarCount) return false;
            if (userData.money < selectData.unlockPrice) return false;
            
            userData.unLockStages.Add(stageId);
            ChangeMoney(-selectData.unlockPrice);
            SaveLoad.SaveUserData(userData);
            return true;
        }

        public void SuccessStage(string stageId, int score)
        {
            int starCount = 0;
            
            if (userData.clearedStages.TryGetValue(stageId, out int value) == false)
            {
                userData.clearedStages.Add(stageId, score);

                for (int i = 0; i < selectStageData.targetScore.Count; i++)
                {
                    if (score < selectStageData.targetScore[i]) break;
                    starCount++;
                }
            }
            else
            {
                userData.clearedStages[stageId] = Mathf.Max(score, value);

                for (int i = 0; i < selectStageData.targetScore.Count; i++)
                {
                    if (value >= selectStageData.targetScore[i]) continue;
                    if (score < selectStageData.targetScore[i]) break;
                    starCount++;
                }
            }
            
            userData.money += score;
            userData.starCount += starCount;
            
            SaveLoad.SaveUserData(userData);
        }
        
        public List<StageData> GetStageData() => stageData;
        
        public bool CheckStageCleared(string stageId) => userData.clearedStages.ContainsKey(stageId);
        public bool CheckStageCleared(string stageId, out int score) => userData.clearedStages.TryGetValue(stageId, out score);
        public bool CheckStageUnLocked(string stageId) => userData.unLockStages.Contains(stageId);
        
        public int GetUserStarCount => userData.starCount;
        public int GetUserMoney => userData.money;
        public string GetUserName => userData.name;

        public void ChangeMoney(int value)
        {
            userData.money += value;
            EventManager.Instance.OnMoneyChanged?.Invoke(userData.money);
        }

        public void ChangeStarCount(int value)
        {
            userData.starCount += value;
            EventManager.Instance.OnStarCountChanged?.Invoke(userData.starCount);
        }

        public void SetSelectStageData(StageData data)
        {
            selectStageData = data;
        }
        
        public StageData GetSelectStageData()
        {
            return selectStageData;
        }

        public StageData FindStageData(string stageId)
        {
            return stageData.Find(s => s.id == stageId);
        }

        public FoodIngredient GetCookedResult(FoodIngredient rawIngredient, CookingMethod method)
        {
            return cookedIngredients.Find(c => c.rawIngredient == rawIngredient && c.requiredMethod.HasFlag(method));
        }

        public bool CheckCompleteCook(List<FoodIngredient> platingIngredients, out FoodData food)
        {
            foreach (var data in selectStageData.foods)
            {
                if (countMap.IsSameElement(platingIngredients, data.requiredMaterials) == false) continue;

                food = data;
                return true;
            }
            
            food = null;
            return false;
        }

        public Order CreateOrder()
        {
            if (selectStageData == false) return null;
            var foods = selectStageData.foods;
            var food = foods[Random.Range(0, foods.Count)];
            float randomLimit = 30 * Random.Range(0.8f, 1.2f);

            Order order = new Order();
            order.id = Guid.NewGuid().ToString();
            order.food = food;
            order.timeLimit = randomLimit;

            return order;
        }

        public FoodData GetFoodData(string foodId) => foodData.Find(f => f.id == foodId);
    }
}
