using System;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainSceneContents
{
    public class StageUIPopup : MonoBehaviour
    {
        public TMP_Text stageName;
        
        public Image stageImage;
        
        public StarInfoUI[] starInfoUIs;
        public TMP_Text requiredStarCount;
        public TMP_Text requiredMoney;

        public Button button;
        public TMP_Text buttonText;

        public Sprite[] starIcons;
        
        private StageData selectStageData;
        
        private DataManager DataManager => DataManager.Instance;

        public void Initialize(StageData data)
        {
            selectStageData = data;
            
            stageName.text = selectStageData.stageName;
            stageImage.sprite = selectStageData.stageImage;
            
            bool isUnlocked = DataManager.CheckStageUnLocked(selectStageData.id);
            bool isCleared = DataManager.CheckStageCleared(selectStageData.id, out int score);
            
            SetRequireUI(isUnlocked);
            SetStarInfoUI(isCleared, score);
            
            //button
            buttonText.text = isUnlocked ? "Start" : "Unlock";
            button.onClick.RemoveAllListeners();
            if (isUnlocked) button.onClick.AddListener(StartStage);
            else button.onClick.AddListener(UnlockStage);
        }

        private void UnlockStage()
        {
            if (DataManager.TryUnlockStage(selectStageData.id) == false) return;
            
            Initialize(selectStageData);
        }

        private void StartStage()
        {
            DataManager.Instance.SetSelectStageData(selectStageData);
            SceneController.LoadScene(selectStageData.stageScene);

            if (PhotonNetwork.IsMasterClient)
            {
                MultiplayManager.Instance.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable());
                MultiplayManager.CallRPC("StartStageRPC",RpcTarget.OthersBuffered,selectStageData.id);
            }
        }

        private void SetStarInfoUI(bool isCleared, int score)
        {
            for (int i = 0; i < starInfoUIs.Length; i++)
            {
                int targetScore = selectStageData.targetScore[i];
                
                starInfoUIs[i].scoreText.text = targetScore.ToString();
                starInfoUIs[i].starImage.sprite = score >= targetScore ? starIcons[0] : starIcons[1];
            }
        }

        private void SetRequireUI(bool isUnlocked)
        {
            string starText = $"RequireStar : {selectStageData.requiredStarCount}";
            string priceText = $"Price : {selectStageData.unlockPrice}";

            if (isUnlocked == false)
            {
                if (DataManager.GetUserStarCount < selectStageData.requiredStarCount)
                    starText = $"<color=red>{starText}</color>";
                if (DataManager.GetUserMoney < selectStageData.unlockPrice)
                    priceText = $"<color=red>{priceText}</color>";
            }
            
            requiredStarCount.text = starText;
            requiredMoney.text = priceText;
        }
    }

    [Serializable]
    public class StarInfoUI
    {
        public Image starImage;
        public TMP_Text scoreText;
    }
}
