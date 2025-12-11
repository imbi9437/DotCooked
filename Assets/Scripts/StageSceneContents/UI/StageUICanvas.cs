using System;
using DG.Tweening;
using Manager;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StageSceneContents.UI
{
    [Serializable]
    public class StageTimerUI
    {
        public Image timerIcon;
        public TMP_Text timerText;
        public Slider timerSlider;
    }

    [Serializable]
    public class StageEndUI
    {
        public Transform panelTransform;
        public TMP_Text resultText;
        public Image[] starImages;
        public Slider slider;
        public Sprite[] starSprites;
    }
    
    //todo : 각종 이벤트 등록및 해지를 Stage 변경이벤트로 위치 변경 필요 : 추후 혹시 모를 이벤트 실행을 방지하기 위해
    public class StageUICanvas : MonoBehaviour
    {
        [SerializeField] private StageTimerUI timerUI;
        [SerializeField] private TMP_Text gainGoldText;
        [SerializeField] private StageEndUI endUI;

        private void Awake()
        {
            EventManager.Instance.OnChangeStageScore += ChangeGainGold;
            
            EventManager.Instance.OnStartStageState += OnDrawStageStateUI;
            EventManager.Instance.OnEndStageState += OnRemoveStageStateUI;

            EventManager.Instance.OnChangeTimeLimit += ChangeTimeLimitUI;
            
            EventManager.Instance.OnEndStage += DrawStageEndUI;
        }
        
        private void OnDestroy()
        {
            if (EventManager.Instance == false) return;
            
            EventManager.Instance.OnChangeStageScore -= ChangeGainGold;
            
            EventManager.Instance.OnStartStageState -= OnDrawStageStateUI;
            EventManager.Instance.OnEndStageState -= OnRemoveStageStateUI;
            
            EventManager.Instance.OnChangeTimeLimit -= ChangeTimeLimitUI;
            
            EventManager.Instance.OnEndStage -= DrawStageEndUI;
        }

        private void OnDrawStageStateUI(StageState state)
        {
            switch (state)
            {
                case StageState.Ready:
                    break;
                case StageState.Start:
                    timerUI.timerIcon.transform.DOLocalRotate(new Vector3(0, 0, 180), 2f).SetLoops(-1, LoopType.Incremental);
                    break;
                case StageState.End:
                    break;
            }
            
        }

        private void OnRemoveStageStateUI(StageState state)
        {
            switch (state)
            {
                case StageState.Ready:
                    break;
                case StageState.Start:
                    timerUI.timerIcon.transform.DOKill();
                    break;
                case StageState.End:
                    break;
            }
        }
        
        private void ChangeGainGold(int value)
        {
            gainGoldText.text = value.ToString("N0");
        }

        private void ChangeTimeLimitUI(float cur, float max)
        {
            timerUI.timerText.text = TimeSpan.FromSeconds(cur).ToString(@"mm\:ss");
            timerUI.timerSlider.value = cur / max;
        }

        private void DrawStageEndUI(int score, StageData stageData)
        {
            endUI.panelTransform.gameObject.SetActive(true);

            bool isSuccess = score >= stageData.targetScore[0];
            endUI.resultText.color = isSuccess ? Color.green : Color.red;
            endUI.resultText.text = isSuccess ? "Success" : "Failed";

            for (int i = 0; i < endUI.starImages.Length; i++)
            {
                bool isOver = score >= stageData.targetScore[i];
                endUI.starImages[i].sprite = isOver ? endUI.starSprites[1] : endUI.starSprites[0];
            }
            
            endUI.slider.maxValue = stageData.targetScore[2];
            endUI.slider.value = 0;
            
            var sequence = DOTween.Sequence();
            var tween = endUI.slider.DOValue(score, 2f).SetEase(Ease.Linear);
            
            sequence.Append(tween);
            sequence.AppendInterval(1f);
            sequence.onComplete += () => SceneController.LoadScene(SceneType.Main);
        }
    }
}
