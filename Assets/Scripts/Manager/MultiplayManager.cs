using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Generic;
using Photon.Pun;
using Photon.Realtime;
using ScriptableObjects;
using UnityEngine;

namespace Manager
{
    public partial class MultiplayManager : PunMonoSingleton<MultiplayManager>
    {
        private static RoomInfo selectedRoom;
        private Room currentRoom;
        
        public bool IsSelectRoom => selectedRoom != null;
        public void SelectRoom(RoomInfo roomInfo) => selectedRoom = roomInfo;
        public Room CurrentRoom => currentRoom;
        
        private void OnDestroy()
        {
            if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
            if (PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
            if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
        }
        
        public override void Initialize()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.NickName = DataManager.Instance.GetUserName;
            base.Initialize();
        }
        public static void ConnectToPhoton()
        {
            if (PhotonNetwork.IsConnected) return;
            EventManager.Instance.OnStartConnectToServer?.Invoke();
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();
        }
        public static void JoinLobby(string lobbyName = "", LobbyType type = LobbyType.Default)
        {
            if (PhotonNetwork.InLobby)
            {
                TypedLobby curLobby = PhotonNetwork.CurrentLobby;
                if (curLobby.Name == lobbyName && curLobby.Type == type) return;

                EventManager.Instance.OnStartLeaveLobby?.Invoke();
                PhotonNetwork.LeaveLobby();
            }
            
            TypedLobby lobby = new TypedLobby(lobbyName, type);
            
            EventManager.Instance.OnStartJoinLobby?.Invoke();
            PhotonNetwork.JoinLobby(lobby);
        }
        public static void JoinSelectRoom()
        {
            EventManager.Instance.OnStartJoinRoom?.Invoke();
            PhotonNetwork.JoinRoom(selectedRoom.Name);
        }
        public static void CreateRoom(string name)
        {
            EventManager.Instance.OnStartCreateRoom?.Invoke();
            TypedLobby lobby = PhotonNetwork.InLobby ? PhotonNetwork.CurrentLobby : TypedLobby.Default;
            PhotonNetwork.CreateRoom(name, new RoomOptions { MaxPlayers = 4 }, lobby);
        }
        public static void LeaveRoom()
        {
            EventManager.Instance.OnStartLeaveRoom?.Invoke();
            PhotonNetwork.LeaveRoom();
        }
        public static void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            CallRPC("StartGameRPC", RpcTarget.AllBuffered);
        }
        public static void CallRPC(string methodName, RpcTarget target, params object[] parameters)
        {
            Instance.photonView.RPC(methodName, target, parameters);
        }
        
        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            EventManager.Instance.OnSuccessConnectToServer?.Invoke();
            
            JoinLobby();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            if (EventManager.Instance == false) return;
            EventManager.Instance.OnFailedConnectToServer?.Invoke(cause);
        }

        public override void OnJoinedLobby()
        {
            EventManager.Instance.OnSuccessJoinLobby?.Invoke();
        }
        public override void OnLeftLobby()
        {
            EventManager.Instance.OnStartLeaveLobby?.Invoke();
        }
        
        
        public override void OnJoinedRoom()
        {
            currentRoom = PhotonNetwork.CurrentRoom;
            int playerId = PhotonNetwork.AllocateViewID(PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"CharID",playerId}});
            
            EventManager.Instance.OnSuccessJoinRoom?.Invoke();
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            EventManager.Instance.OnFailedJoinRoom?.Invoke();
            Debug.Log($"<color=red>Failed Join Room : {returnCode} {message}</color>");
        }
        
        
        public override void OnCreatedRoom()
        {
            currentRoom = PhotonNetwork.CurrentRoom;
            EventManager.Instance.OnSuccessCreateRoom?.Invoke();
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            EventManager.Instance.OnFailedCreateRoom?.Invoke();
            Debug.Log($"<color=red>Failed Create Room : {returnCode} {message}</color>");
        }


        public override void OnLeftRoom()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnLeaveRoom?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            EventManager.Instance.OnPlayerJoinRoom?.Invoke(newPlayer);
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            EventManager.Instance.OnPlayerLeaveRoom?.Invoke(otherPlayer);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            EventManager.Instance.OnUpdateRoomList?.Invoke(roomList);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            EventManager.Instance.OnRoomPropertiesUpdate?.Invoke(propertiesThatChanged);
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            Debug.Log("Master Client Switched :");
            EventManager.Instance.OnMasterClientSwitched?.Invoke(newMasterClient);
        }

        #endregion
    }
}
