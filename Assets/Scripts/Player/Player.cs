using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Interface;
using Manager;
using Photon.Pun;
using StageSceneContents.ContentsObject;
using UnityEngine;

public partial class Player : MonoBehaviourPunCallbacks, IGrabber, IInteractor
{
    private static readonly int IsMove = Animator.StringToHash("IsMove");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Take = Animator.StringToHash("Take");
    private static readonly int PutDown = Animator.StringToHash("PutDown");
    private static readonly int Throw = Animator.StringToHash("Throw");
    private static readonly int IsCook = Animator.StringToHash("IsCook");
    private static readonly int CookType = Animator.StringToHash("CookType");
    private static readonly int Emergency = Animator.StringToHash("Emergency");
    
    private SpriteRenderer mainRenderer;
    private Animator mainAnimator;
    private Rigidbody2D mainRigidbody;
    private Collider2D mainCollider;
    
    private bool isRun;
    public float defaultSpeed;
    private float calcSpeed;
    
    private Vector2 curDir;

    public LayerMask interactableLayer;

    public IInteractable SelectedInteractable { get; set; }
    
    public Transform GrabPivot { get; set; }
    public IGrabAble GrabAble { get; set; }
    
    public Vector2 ReleaseVector { get; set; }

    private void Awake()
    {
        mainRigidbody = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<Collider2D>();
        mainAnimator = GetComponent<Animator>();
        mainRenderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        
        GrabPivot = transform.Find("GrabPivot");

        interactableLayer = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("IgnorePhysics"));
    }

    private void Start()
    {
        if (DataManager.Instance.GetSelectStageData() != null)
        {
            if (photonView.IsMine == false && PhotonNetwork.InRoom)
                GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
        }
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom && photonView.IsMine == false)
        {
            float x = mainAnimator.GetFloat(Horizontal);
            float y = mainAnimator.GetFloat(Vertical);
            curDir = new Vector2(x, y).normalized;
            Hovering();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        
        if (EventManager.Instance == false) return;
        EventManager.Instance.OnMove -= Move;
        EventManager.Instance.OnIdle -= Idle;
        EventManager.Instance.OnRun -= IsRun;

        EventManager.Instance.OnInteract -= Interaction;
        EventManager.Instance.OnHovering -= Hovering;

        EventManager.Instance.OnPlayerLeaveRoom -= PlayerLeaveEvent;
    }

    public void InitEvent()
    {
        EventManager.Instance.OnMove += Move;
        EventManager.Instance.OnIdle += Idle;
        EventManager.Instance.OnRun += IsRun;
        
        EventManager.Instance.OnInteract += Interaction;
        EventManager.Instance.OnHovering += Hovering;
        
        EventManager.Instance.OnPlayerLeaveRoom += PlayerLeaveEvent;
    }
}
