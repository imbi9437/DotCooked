using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TitleSceneContents.UI
{
    public class CreateRoomPanel : UIPanel
    {
        public override int PanelIndex => (int)MultiplayUIPanel.CreateRoom;
        [SerializeField] private TMP_InputField roomNameInputField;

        private void Awake()
        {
            roomNameInputField.onSubmit.AddListener(CreateRoom);
        }

        private void OnDestroy()
        {
            roomNameInputField.onSubmit.RemoveAllListeners();
        }

        private void CreateRoom(string name)
        {
            Controller.ClosePanel();
            MultiplayManager.CreateRoom(name);
        }

        
    }
}
