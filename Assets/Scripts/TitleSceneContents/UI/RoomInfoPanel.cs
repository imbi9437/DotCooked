using Manager;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TitleSceneContents.UI
{
    public class RoomInfoPanel : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text playerCount;
    
        private RoomInfo roomInfo;

        private void Awake()
        {
            toggle.group = transform.parent.GetComponent<ToggleGroup>();
        }

        public void Initialize(RoomInfo roomInfo)
        {
            this.roomInfo = roomInfo;
            roomName.text = roomInfo.Name;
            playerCount.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(SelectRoom);
        }

        private void SelectRoom(bool isSelected)
        {
            MultiplayManager.Instance.SelectRoom(isSelected ? roomInfo : null);
        }
    }
}
