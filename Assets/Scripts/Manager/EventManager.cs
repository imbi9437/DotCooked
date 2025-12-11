using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using Interface;
using MainSceneContents;
using Photon.Realtime;
using ScriptableObjects;
using StageSceneContents;
using StageSceneContents.Interactor;
using UnityEngine;

namespace Manager
{
    public class EventManager : MonoSingleton<EventManager>
    {
        public Action OnCompleteManagerInitialize;
        public Action<UserData> OnLoadUserData;

        public Action<bool, StageInteractor> OnStageInteract;

        public Action OnBeforeLoadScene;
        
        //UserData Value Changed Event
        public Action<int> OnStarCountChanged;
        public Action<int> OnMoneyChanged;
        
        //Movement Input Event
        public Action<Vector2> OnMove;
        public Action<Vector2> OnIdle;
        public Action<bool> OnRun;
        
        //Interaction Input Event
        public Action OnHovering;
        public Action OnInteract;

        public Action OnCancel;

        #region StageEvent

        public Action<Order> OnFailedOrder;
        public Action<Order> OnSuccessOrder;
        public Action<Order> OnGetOrder;
        public Action<FoodData> OnServeFood;
        
        public Action<float, float> OnChangeTimeLimit;
        public Action<int> OnChangeStageScore;
        public Action<int, StageData> OnEndStage;

        public Action<StageState> OnStartStageState;
        public Action<StageState> OnEndStageState;

        #endregion

        #region Multiplay Event

        public Action OnStartConnectToServer;
        public Action OnSuccessConnectToServer;
        public Action<DisconnectCause> OnFailedConnectToServer;
        
        public Action OnStartJoinLobby;
        public Action OnSuccessJoinLobby;
        
        public Action OnStartLeaveLobby;
        
        public Action OnStartCreateRoom;
        public Action OnSuccessCreateRoom;
        public Action OnFailedCreateRoom;
        
        public Action OnStartJoinRoom;
        public Action OnSuccessJoinRoom;
        public Action OnFailedJoinRoom;
        
        public Action OnStartLeaveRoom;
        public Action OnLeaveRoom;
        
        public Action<Photon.Realtime.Player> OnPlayerJoinRoom;
        public Action<Photon.Realtime.Player> OnPlayerLeaveRoom;
        
        public Action<Photon.Realtime.Player> OnMasterClientSwitched;

        public Action<List<RoomInfo>> OnUpdateRoomList;
        
        public Action<ExitGames.Client.Photon.Hashtable> OnRoomPropertiesUpdate;

        #endregion
    }
}