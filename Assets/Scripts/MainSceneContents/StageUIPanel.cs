using System;
using Manager;
using Photon.Pun;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MainSceneContents
{
    public class StageUIPanel : MonoBehaviour
    {
        private Canvas canvas;
        private StageUIPopup popup;
        private GraphicRaycaster raycaster;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            popup = GetComponentInChildren<StageUIPopup>();
            raycaster = GetComponent<GraphicRaycaster>();

            ToggleRaycaster(PhotonNetwork.MasterClient);
        }

        private void Start()
        {
            canvas.enabled = false;
            EventManager.Instance.OnStageInteract += ShowStagePopup;
            EventManager.Instance.OnMasterClientSwitched += ToggleRaycaster;
        }

        private void OnDestroy()
        {
            if (EventManager.Instance == false) return;
            EventManager.Instance.OnStageInteract -= ShowStagePopup;
            EventManager.Instance.OnMasterClientSwitched -= ToggleRaycaster;
        }

        private void ShowStagePopup(bool isInteract,StageInteractor interactor)
        {
            if (Camera.main == null) return;
            canvas.enabled = isInteract;
            Vector2 pos = Camera.main.WorldToScreenPoint(interactor.transform.position);
            popup.transform.position = pos;
            popup.Initialize(interactor.stageData);
        }

        private void ToggleRaycaster(Photon.Realtime.Player masterPlayer)
        {
            if (PhotonNetwork.InRoom == false) return;
            
            raycaster.enabled = PhotonNetwork.LocalPlayer.ActorNumber == masterPlayer.ActorNumber;
        }
    }
}
