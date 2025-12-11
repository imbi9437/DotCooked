using System;
using System.Collections.Generic;
using Manager;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TitleSceneContents.UI
{
    public class RoomListPanel : UIPanel
    {
        public override int PanelIndex => (int)MultiplayUIPanel.RoomList;
        
        [SerializeField] private Transform content;
        [SerializeField] private RoomInfoPanel infoSubPanelPrefab;

        [SerializeField] private Button joinButton;
        [SerializeField] private Button createButton;

        private Queue<RoomInfoPanel> panelQueue = new Queue<RoomInfoPanel>();
        private Dictionary<string, RoomInfoPanel> roomInfoPanelDic = new Dictionary<string, RoomInfoPanel>();

        private void OnDestroy()
        {
            joinButton.onClick.RemoveAllListeners();
            createButton.onClick.RemoveAllListeners();
            
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnUpdateRoomList -= ValidateRooms;
        }

        public override void Initialize(UIController controller)
        {
            base.Initialize(controller);
            EventManager.Instance.OnUpdateRoomList += ValidateRooms;
            
            joinButton.onClick.AddListener(JoinSelectRoom);
            createButton.onClick.AddListener(CreateRoom);
        }

        private void ValidateRooms(List<RoomInfo> rooms)
        {
            foreach (var roomInfo in rooms)
            {
                if (roomInfo.RemovedFromList == false)
                    ValidatePanel(roomInfo);
                else if (roomInfoPanelDic.ContainsKey(roomInfo.Name))
                    RemovePanel(roomInfo.Name);
            }
        }

        #region Button Event


        private void JoinSelectRoom()
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (MultiplayManager.Instance.IsSelectRoom == false) return;
            MultiplayManager.JoinSelectRoom();
        }

        private void CreateRoom()
        {
            EventSystem.current.SetSelectedGameObject(null);
            Controller.OpenPanel((int)MultiplayUIPanel.CreateRoom);
        }
        
        #endregion
        
        #region RoomInfoPanel Event

        
        private void RemovePanel(string key)
        {
            var temp = roomInfoPanelDic[key];
            temp.gameObject.SetActive(false);
            roomInfoPanelDic.Remove(key);
            panelQueue.Enqueue(temp);
        }

        private void ValidatePanel(RoomInfo roomInfo)
        {
            if (roomInfoPanelDic.ContainsKey(roomInfo.Name) == false)
                AddPanel(roomInfo);
            else
                ChangePanel(roomInfo);
        }

        private void AddPanel(RoomInfo roomInfo)
        {
            RoomInfoPanel panel = null;
        
            if (panelQueue.Count <= 0)
                panel = Instantiate(infoSubPanelPrefab, content);
            else
                panel = panelQueue.Dequeue();
        
            panel.Initialize(roomInfo);
            panel.gameObject.SetActive(true);
            
            roomInfoPanelDic.Add(roomInfo.Name, panel);
        }

        private void ChangePanel(RoomInfo roomInfo)
        {
            RoomInfoPanel panel = roomInfoPanelDic[roomInfo.Name];
            panel.Initialize(roomInfo);
        }
        
        
        #endregion

        
    }
}
