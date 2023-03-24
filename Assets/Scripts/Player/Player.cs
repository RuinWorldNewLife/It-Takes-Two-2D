using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using Cinemachine;

public class Player : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private PlayerSelfData playerSelfData;
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerExternalAffect ExternalAffect { get; private set; }

    #region 一些变量
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;
    private Vector3 posPlayerClone;
    public bool IsMinePlayer { get;private set; }
    private bool updateInvokeAvoid;//避免多次执行协程

    public int FacingDirection { get; private set; }
    [HideInInspector]
    public bool isRotate;
    [HideInInspector]
    public bool isFly;
    [HideInInspector]
    public float flySpeed;
    [HideInInspector]
    public Vector2 windToFlyDirection;
    [HideInInspector]
    public float exitWindToFlyTime;
    [HideInInspector]
    public float exitWindToFlyStartTime;
    [HideInInspector]
    public bool isInJumpOrDashState;
    [HideInInspector]
    public bool isJumpPlat;//是否在跳板上跳跃
    [HideInInspector]
    public float jumpPlatTime;
    [HideInInspector]
    public float jumpPlatForce;
    [HideInInspector]
    public float playerTempJumpSpeed;
    #endregion

    #region 状态
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public WallHandleState wallHandleState { get; private set; }
    public WallSlide wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public PlayerRotateState RotateState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerFlyState FlyState { get; private set; }
    public PlayerJumpPlatState JumpPlatState { get; private set; }

    #endregion

    #region 组件
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }

    [SerializeField]
    private Transform checkGround;
    //Photon主要类,PhotonView脚本对象
    private PhotonView photonView;
    [SerializeField]
    private Transform wallCheck;
    #endregion
    #region unity的回调函数
    private void Awake() 
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "In Air");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "In Air");
        wallHandleState = new WallHandleState(this, StateMachine, playerData, "wallHandle");
        wallSlideState = new WallSlide(this, StateMachine, playerData, "wallSlide");
        wallJumpState = new WallJumpState(this, StateMachine, playerData, "In Air");
        RotateState = new PlayerRotateState(this, StateMachine, playerData, "Rotate");
        DashState = new PlayerDashState(this, StateMachine, playerData, "Fly");
        FlyState = new PlayerFlyState(this, StateMachine, playerData, "idle");
        JumpPlatState = new PlayerJumpPlatState(this, StateMachine, playerData, "In Air");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        ExternalAffect = GetComponent<PlayerExternalAffect>();
        photonView = GetComponent<PhotonView>();
        CinemachineVirtualCamera cinemachine = GameObject.Find("Camera").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = transform;
        photonView.RPC("CameraFollow", RpcTarget.Others);///相机跟随
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;//记录任务朝向
        isJumpPlat = false;
        isRotate = false;
        isInJumpOrDashState = false;
        updateInvokeAvoid = true;
        playerData.ifHaveDashKey = false;//初始化是否拥有闪避钥匙。
        playerData.ifHaveWallKey = false;//初始化是否拥有爬墙技能。
        playerData.amountOfJump = 1;//初始化能够跳跃一次。
        IsMinePlayer = photonView.IsMine;//判断当前角色是否为本身。
        RB.gravityScale = playerData.gravityValue;
        exitWindToFlyTime = playerData.exitWindToFlyTime;
        //将PhotonNetWork的通讯速率改为每帧60秒
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
    }
    [PunRPC]
    public void CameraFollow()
    {
        CinemachineVirtualCamera cinemachine = GameObject.Find("Camera").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = transform;
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
        if (isInJumpOrDashState && updateInvokeAvoid)
        {
            updateInvokeAvoid = false;
            MonoHelper.Instance.WaitSomeTimeInvoke(() => { }, 0, () => { return true; });//如果有协程，停止协程
            MonoHelper.Instance.WaitSomeTimeInvoke(() => { isInJumpOrDashState = false; 
            updateInvokeAvoid = true;
            }, 0.2f, () => { return false; });
        }

        //如果当前对象不是自己，则将自己的位置信息平滑地传输到另外一个客户端
        //保证玩家在游玩时候任务移动不会出现卡顿
        if (!photonView.IsMine)
        {
            Player2SmoothPostion();
        }
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region 检查方法
    public bool CheckIfIsGrounded()
    {
        return Physics2D.OverlapCircle(
            checkGround.position,
            playerData.circleRadius,
            playerSelfData.whatIsGround) ;
    }
    /// <summary>
    /// Scene视图检查方法
    /// </summary>
    public void CheckIfShouldFlip(float xInput)
    {
        if (xInput != 0 && !(Mathf.Abs(xInput - FacingDirection) < 0.01f))
        {
            Flip();
        }
    }
    //设置是否碰到墙壁。
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerSelfData.whatIsGround) && playerData.ifHaveWallKey;
    }
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerSelfData.whatIsGround) && playerData.ifHaveWallKey;
    }

    #endregion

    #region 设置方法
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    /// <summary>
    /// 设置触碰到跳跃钥匙的方法。
    /// </summary>
    public void SetTouchingJumpKey()
    {
        playerData.amountOfJump++;//增加最大跳跃次数
        JumpState.lastAmountOfJump++;//添加一次当前的临时跳跃次数
    }
    /// <summary>
    /// 设置触碰到闪避钥匙方法
    /// </summary>
    public void SetTouchingDashKey()
    {
        playerData.ifHaveDashKey = true;
    }
    /// <summary>
    /// 设置触碰到爬墙钥匙方法
    /// </summary>
    public void SetTouchingWallKey()
    {
        playerData.ifHaveWallKey = true;
    }
    /// <summary>
    /// 设置重力缩放
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void SetGravityScale(float value)
    {
        RB.gravityScale = value;
    }

    public void SetGravityScaleFun(float value,RpcTarget rpcTarget)
    {
        photonView.RPC("SetGravityScale", rpcTarget, value);
    }
    /// <summary>
    /// 设置是否物理失效
    /// </summary>
    /// <param name="isKinematicValue"></param>
    [PunRPC]
    public void SetRBisKinematic(bool isKinematicValue)
    {
        RB.isKinematic = isKinematicValue;
    }
    public void SetRBisKinematicFun(bool value, RpcTarget rpcTarget)
    {
        photonView.RPC("SetRBisKinematic", rpcTarget, value);
    }

    /// <summary>
    /// 网络连接平滑方法
    /// </summary>
    public void Player2SmoothPostion()
    {
        transform.position = Vector3.Lerp(transform.position, posPlayerClone, 15.0f * Time.deltaTime);
    }
    #endregion
    #region 其他方法
    /// <summary>
    /// 翻转方法
    /// </summary>
    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(new Vector3(0, 180, 0));
    }
    /// <summary>
    /// 重置跳跃次数代码
    /// </summary>
    public void resetLastAmountOfJump() => JumpState.lastAmountOfJump = playerData.amountOfJump;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //正在传输的时候，传输自己的坐标信息
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        //正在读取的时候，将读取到的信息强转为Vector3，并且接收。
        if (stream.IsReading)
        {
            posPlayerClone = (Vector3)stream.ReceiveNext();
        }
    }

    #endregion
}
