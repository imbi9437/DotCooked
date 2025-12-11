using System;
using System.Collections.Generic;
using Manager;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TitleSceneContents.UI
{
    public enum MultiplayUIPanel
    {
        Default = 0,
        Loading = 1,
        RoomList = 2,
        CurrentRoom = 3,
        CreateRoom = 4,
    }
    
    public class MultiplayUI : UIController
    {
        [SerializeField] Animator playerIconAnimator;
        
        protected override void Start()
        {
            base.Start();
            playerIconAnimator.SetFloat("Vertical",-1f);
            EventManager.Instance.OnCancel += ReturnPanel;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnCancel -= ReturnPanel;
        }

        public override void OpenUI()
        {
            base.OpenUI();
            
            if (PhotonNetwork.IsConnected == false)
                MultiplayManager.ConnectToPhoton();
            else OpenPanel((int)MultiplayUIPanel.RoomList);
        }

        public override void CloseUI()
        {
            base.CloseUI();
            
            ReturnAllPanel();
            
            if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
        }
    }
}
