using System;
using System.Collections;
using System.Collections.Generic;
using Generic;
using Manager;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    private Vector2 lastMoveInput;
    private Vector2 moveInput;
    private bool isMoving;
    private bool isRun;

    private void Update()
    {
        UpdateMoveInput();
        UpdateRunInput();
        UpdateInteractInput();
        UpdateEscapeInput();    
    }

    private void UpdateMoveInput()
    {
        bool horizontal = Input.GetButton("Horizontal");
        bool vertical = Input.GetButton("Vertical");

        isMoving = horizontal || vertical;        
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        if (isMoving) lastMoveInput = moveInput;
        
        if (isMoving) EventManager.Instance.OnMove?.Invoke(moveInput);
        else EventManager.Instance.OnIdle?.Invoke(lastMoveInput);
    }

    private void UpdateRunInput()
    {
        isRun = Input.GetButton("Run");
        
        EventManager.Instance.OnRun?.Invoke(isRun);
    }

    private void UpdateInteractInput()
    {
        EventManager.Instance.OnHovering?.Invoke();
        if (Input.GetButtonDown("Interact")) EventManager.Instance.OnInteract?.Invoke();
    }
    
    private void UpdateEscapeInput()
    {
        if (Input.GetButtonDown("Cancel")) EventManager.Instance?.OnCancel?.Invoke();   
    }
}
