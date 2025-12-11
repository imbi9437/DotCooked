using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TitleSceneContents.UI
{
    [Serializable]
    public class PlayerInfoUI
    {
        public Transform uiTransform;
        public TMP_Text playerName;
        public RawImage playerIcon;
    }
    
    public class CurrentRoomPanel : UIPanel
    {
        public override int PanelIndex => (int)MultiplayUIPanel.CurrentRoom;
        
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text playerCount;
        
        [SerializeField] private Button startButton;
        [SerializeField] private Button leaveButton;
        
        [SerializeField] private List<PlayerInfoUI> playerInfoUIs;

        private bool canLeaveRoom;

        private void OnDisable()
        {
            if (canLeaveRoom) LeaveCurrentRoom();
        }

        private void OnDestroy()
        {
            leaveButton.onClick.RemoveAllListeners();
            startButton.onClick.RemoveAllListeners();
            
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnSuccessCreateRoom -= InitRoom;
            EventManager.Instance.OnSuccessJoinRoom -= InitRoom;
            
            EventManager.Instance.OnPlayerLeaveRoom -= RemovePlayer;
            EventManager.Instance.OnPlayerJoinRoom -= AddPlayer;
            
            EventManager.Instance.OnBeforeLoadScene -= ChangeCanLeave;
        }

        public override void Initialize(UIController controller)
        {
            base.Initialize(controller);

            EventManager.Instance.OnSuccessCreateRoom += InitRoom;
            EventManager.Instance.OnSuccessJoinRoom += InitRoom;
            
            EventManager.Instance.OnPlayerLeaveRoom += RemovePlayer;
            EventManager.Instance.OnPlayerJoinRoom += AddPlayer;

            EventManager.Instance.OnBeforeLoadScene += ChangeCanLeave;
            
            leaveButton.onClick.AddListener(LeaveCurrentRoom);
            startButton.onClick.AddListener(MultiplayManager.StartGame);

            canLeaveRoom = true;
        }

        private void InitRoom()
        {
            Room room = MultiplayManager.Instance.CurrentRoom;
            
            roomName.text = room.Name;
            playerCount.text = $"{room.PlayerCount}/{room.MaxPlayers}";

            startButton.interactable = PhotonNetwork.IsMasterClient;
            
            foreach (var infoUI in playerInfoUIs)
            {
                infoUI.uiTransform.gameObject.SetActive(false);
            }

            foreach (var kvp in room.Players)
            {
                int index = kvp.Key - 1;
                playerInfoUIs[index].uiTransform.gameObject.SetActive(true);
                playerInfoUIs[index].playerName.text = kvp.Value.NickName;
            }
            
            if (gameObject.activeSelf == false)
                Controller.OpenPanel(PanelIndex);
        }

        private void AddPlayer(Photon.Realtime.Player player)
        {
            int index = player.ActorNumber - 1;
            playerInfoUIs[index].uiTransform.gameObject.SetActive(true);
            playerInfoUIs[index].playerName.text = player.NickName;
        }

        private void RemovePlayer(Photon.Realtime.Player player)
        {
            int index = player.ActorNumber - 1;
            playerInfoUIs[index].uiTransform.gameObject.SetActive(false);
        }

        private void LeaveCurrentRoom()
        {
            if (PhotonNetwork.InRoom == false) return;
            MultiplayManager.LeaveRoom();
        }

        private void ChangeCanLeave()
        {
            canLeaveRoom = !canLeaveRoom;
        }
    }
}
