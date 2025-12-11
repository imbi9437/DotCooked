using System;
using Manager;
using Photon.Realtime;
using TMPro;

namespace TitleSceneContents.UI
{
    public class MultiLoadingPanel : UIPanel
    {
        public override int PanelIndex => (int)MultiplayUIPanel.Loading;
        public TMP_Text text;

        private void OnDestroy()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnStartConnectToServer -= StartLoading;
            EventManager.Instance.OnSuccessConnectToServer -= CompleteLoading;
            EventManager.Instance.OnFailedConnectToServer -= FailedLoading;
        }

        public override void Initialize(UIController controller)
        {
            base.Initialize(controller);

            EventManager.Instance.OnStartConnectToServer += StartLoading;
            EventManager.Instance.OnSuccessConnectToServer += CompleteLoading;
            EventManager.Instance.OnFailedConnectToServer += FailedLoading;
        }
        
        private void StartLoading()
        {
            Controller.OpenPanel(PanelIndex);
            text.text = "Connecting to server...";
        }

        private void CompleteLoading()
        {
            Controller.ChangePanel((int)MultiplayUIPanel.RoomList);
        }

        private void FailedLoading(DisconnectCause cause)
        {
            text.text = "Failed to connect to server";
        }
    }
}
