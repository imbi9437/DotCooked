using ExitGames.Client.Photon;
using Manager;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace SceneScripts
{
    public class MainScene : MonoBehaviour
    {
        public TMP_Text moneyText;
        public TMP_Text startCountText;
        public Transform spawnTransform;

        private void Start()
        {
            EventManager.Instance.OnMoneyChanged += ChangeMoney;
            EventManager.Instance.OnStarCountChanged += ChangeStartCount;
            EventManager.Instance.OnMasterClientSwitched += CreateMainCharacter;
            
            moneyText.text = DataManager.Instance.GetUserMoney.ToString();
            startCountText.text = DataManager.Instance.GetUserStarCount.ToString();

            CreateMainCharacter(PhotonNetwork.MasterClient);
        }

        private void OnDestroy()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.OnMoneyChanged -= ChangeMoney;
            EventManager.Instance.OnStarCountChanged -= ChangeStartCount;
            EventManager.Instance.OnMasterClientSwitched -= CreateMainCharacter;
        }

        private void ChangeMoney(int money)
        {
            moneyText.text = money.ToString();
        }

        private void ChangeStartCount(int count)
        {
            startCountText.text = count.ToString();       
        }

        private void CreateMainCharacter(Photon.Realtime.Player player)
        {
            if (PhotonNetwork.InRoom == false)
                GameManager.Instance.CreatePlayer(spawnTransform.position, true, 0);
            else
            {
                bool isMine = PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber;
                int id = (int)player.CustomProperties["CharID"];
                
                GameManager.Instance.CreatePlayer(spawnTransform.position, isMine, id);
            }
        }
    }
}
