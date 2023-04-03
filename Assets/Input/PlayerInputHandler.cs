using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInputHandler : MonoBehaviourPunCallbacks
{
    //方便读取目前用户的输入
    private PlayerInput playerInput;
    private Camera cam;
    public Vector2 Movement { get; private set; }
    public float NormInputX { get; private set; }
    public float NormInputY { get; private set; }
    public bool JumpInputStop { get; private set; }
    
    public bool DashInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool HandleInput { get; private set; }
    //设置能否控制角色的变量
    public bool CanControl { get; private set; }
    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    [SerializeField]
    private float dashinputHoldTime = 0.2f;
    private float dashInputStartTime;
    private void Start()
    {
        CanControl = true;//初始化角色可以控制
        DashInput = false;
        JumpInput = false;
        HandleInput = false;
        JumpInputStop = true;
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }
    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine)//如果正在操作的不是本身，则返回
            return;
        if (!CanControl) //如果不能控制，则返回
        {
            NormInputX = 0;
            NormInputY = 0;
            return;
        }
            
        Movement = context.ReadValue<Vector2>();
        if (Mathf.Abs(Movement.x) > 0.5f)
        {
            NormInputX = (Movement * Vector2.right).normalized.x;
        }
        else
        {
            NormInputX = 0;
        }
        if (Mathf.Abs(Movement.y) > 0.5f)
        {
            NormInputY = (Movement * Vector2.up).normalized.y;
        }
        else
        {
            NormInputY = 0;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine)//如果正在操作的不是本身，则返回
            return;
        if (!CanControl)//如果不能控制，则返回
            return;
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine)//如果正在操作的不是本身，则返回
            return;
        if (!CanControl)//如果不能控制，则返回
            return;
        if (context.started)
        {
            DashInput = true;
            dashInputStartTime = Time.time;
        }
    }

    public void OnHandleInput(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine)//如果正在操作的不是本身，则返回
            return;
        if (!CanControl)//如果不能控制，则返回
            return;
        if (context.started)
        {
            HandleInput = true;
        }
        if (context.canceled)
        {
            HandleInput = false;
        }
    }
    


    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + dashinputHoldTime)
        {
            DashInput = false;
        }
    }
    /// <summary>
    /// 角色停止控制方法，true为可控制，false为不可控制
    /// </summary>
    public void ControlHandler(bool controlHandle)
    {
        CanControl = controlHandle;
    }
}
