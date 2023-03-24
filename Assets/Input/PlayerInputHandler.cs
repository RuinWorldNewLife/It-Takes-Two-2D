using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInputHandler : MonoBehaviourPunCallbacks
{
    //�����ȡĿǰ�û�������
    private PlayerInput playerInput;
    private Camera cam;
    public Vector2 Movement { get; private set; }
    public float NormInputX { get; private set; }
    public float NormInputY { get; private set; }
    public bool JumpInputStop { get; private set; }
    
    public bool DashInput { get; private set; }
    public bool JumpInput { get; private set; }
    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    [SerializeField]
    private float dashinputHoldTime = 0.2f;
    private float dashInputStartTime;
    private void Start()
    {
        DashInput = false;
        JumpInput = false;
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
        if (!photonView.IsMine)//������ڲ����Ĳ��Ǳ������򷵻�
            return;
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
        if (!photonView.IsMine)//������ڲ����Ĳ��Ǳ������򷵻�
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
        if (!photonView.IsMine)//������ڲ����Ĳ��Ǳ������򷵻�
            return;
        if (context.started)
        {
            DashInput = true;
            dashInputStartTime = Time.time;
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
    
}