using System;
using Manager;
using TitleSceneContents.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingUI
{
    public class SettingUIController : UIController
    {
        [SerializeField] private Button exitButton;
        

        protected override void Start()
        {
            base.Start();

            EventManager.Instance.OnCancel += ToggleUI;
            exitButton.onClick.AddListener(Application.Quit);
            
            CloseUI();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            exitButton.onClick.RemoveAllListeners();
            
            if (EventManager.Instance == false) return;
            EventManager.Instance.OnCancel -= ToggleUI;
        }
    }
}
