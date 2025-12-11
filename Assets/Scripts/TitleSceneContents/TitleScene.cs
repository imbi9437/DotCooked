using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Manager;
using ScriptableObjects;
using TitleSceneContents.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneScripts
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private TMP_Text anyKeyText;
        [SerializeField] private TMP_Text titleText;
        
        [SerializeField] private List<Button> menuButtons; //[ Start Single, Start Multi, Setting, Exit]

        [SerializeField] private MultiplayUI multiplayUI;
        
        private IEnumerable<Graphic> buttonGraphics;

        private void Awake()
        {
            anyKeyText.color = new Color(1,1,1,0);
            titleText.color = new Color(1,1,1,0);
            
            buttonGraphics = menuButtons.SelectMany(b => b.GetComponentsInChildren<Graphic>());

            foreach (var buttonGraphic in buttonGraphics)
            {
                buttonGraphic.DOFade(0, 0f);
            }
        }

        private void Start()
        {
            EventManager.Instance.OnCompleteManagerInitialize += ShowAnyKeyText;
        }

        private void ShowAnyKeyText()
        {
            anyKeyText.DOFade(1, 1f);
            WaitingInput().Forget();
        }

        private async UniTaskVoid WaitingInput()
        {
            await UniTask.WaitUntil(() => Input.anyKeyDown);

            Sequence sequence = DOTween.Sequence();

            var fadeTween = titleText.DOFade(1, 3f);
            var titleMoveTween = titleText.transform.DOLocalMove(Vector3.zero, 3f);
            var removeTween = anyKeyText.DOFade(0, 1f);
            
            sequence.Append(titleMoveTween);
            sequence.Join(fadeTween);
            sequence.Join(removeTween);
            
            bool isFirst = true;
            
            foreach (var graphic in buttonGraphics)
            {
                var tween = graphic.DOFade(1, 2f);
                
                if (isFirst)
                {
                    sequence.Append(tween);
                    isFirst = false;
                }
                else
                {
                    sequence.Join(tween);
                }
            }

            sequence.onComplete += RegisterButtonEvent;
        }

        private void RegisterButtonEvent()
        {
            menuButtons[0].onClick.AddListener(() => SceneController.LoadScene(SceneType.Main));
            menuButtons[1].onClick.AddListener(multiplayUI.OpenUI);
            // menuButtons[2].onClick.AddListener();
            menuButtons[3].onClick.AddListener(Application.Quit);
        }
    }
}
