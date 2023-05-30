using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    private Transform handleTF;//handle子物体
    private CinemachineVirtualCamera cinemachine;//虚拟摄像机

    public bool IsMinePlayer { get; private set; }
    private bool updateInvokeAvoid;//避免多次执行协程

    private bool preHandleInput;//上一帧是否有按下handle键。

    private Material _myMaterial;//该角色的材质
    private float fadeTimer = -0.1f;//计时器
    [HideInInspector]
    public bool isDead;



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
    public PlayerTurnInState TurnInState { get; private set; }

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
        JumpState = new PlayerJumpState(this, StateMachine, playerData, playerSelfData, "In Air");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "In Air");
        wallHandleState = new WallHandleState(this, StateMachine, playerData, "wallHandle");
        wallSlideState = new WallSlide(this, StateMachine, playerData, "wallSlide");
        wallJumpState = new WallJumpState(this, StateMachine, playerData, playerSelfData, "In Air");
        RotateState = new PlayerRotateState(this, StateMachine, playerData, playerSelfData, "Rotate");
        DashState = new PlayerDashState(this, StateMachine, playerData, playerSelfData, "Fly");
        FlyState = new PlayerFlyState(this, StateMachine, playerData, playerSelfData, "idle");
        JumpPlatState = new PlayerJumpPlatState(this, StateMachine, playerData, "In Air");
        TurnInState = new PlayerTurnInState(this, StateMachine, playerData, "TurnIn");
    }

    private void Start()
    {

        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        ExternalAffect = GetComponent<PlayerExternalAffect>();
        photonView = GetComponent<PhotonView>();
        cinemachine = GameObject.Find("Camera").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        handleTF = transform.GetChild(2);//拿到第三个子物体HandlePos
        photonView.RPC("SetHandleEffectActive", RpcTarget.Others, false);
        SetHandleEffectActiveNotRPC(false);//将第三个子物体的Handle初始化为false。
        if(photonView.IsMine)
        {
            cinemachine.Follow = transform;//摄像机跟随玩家
        }
        photonView.RPC("CameraFollow", RpcTarget.Others);///相机跟随
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;//记录任务朝向
        isJumpPlat = false;
        isRotate = false;
        isInJumpOrDashState = false;
        updateInvokeAvoid = true;
        //playerSelfData.ifHaveDashKey = false;//初始化是否拥有闪避钥匙。
        //playerSelfData.ifHaveWallKey = false;//初始化是否拥有爬墙技能。
        //playerSelfData.ifHaveJumpKey = false;//初始化是否拥有连跳技能。
        //preHandleInput = false;//初始化上一帧handle输入。
        //playerSelfData.amountOfJump = 1;//初始化能够跳跃一次。
        IsMinePlayer = photonView.IsMine;//判断当前角色是否为本身。
        RB.gravityScale = playerData.gravityValue;
        exitWindToFlyTime = playerData.exitWindToFlyTime;
        //将PhotonNetWork的通讯速率改为每帧60秒
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        _myMaterial = GetComponent<SpriteRenderer>().material;//拿到特效材质
        photonView.RPC("Fade", RpcTarget.All, 0.8f);
        isDead = false;//设置玩家并非死亡
        fadeTimer = 0.8f;//初始化消逝计时器
        StartCoroutine(FadeAndAppear(false, -0.1f));
    }


    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
        if (isInJumpOrDashState && updateInvokeAvoid)
        {
            updateInvokeAvoid = false;
            MonoHelper.Instance.WaitSomeTimeInvoke(() => { }, 0, () => { return true; });//如果有协程，停止协程
            MonoHelper.Instance.WaitSomeTimeInvoke(() =>
            {
                isInJumpOrDashState = false;
                updateInvokeAvoid = true;
            }, 0.2f, () => { return false; });
        }

        CheckIfHandleInputChange(); //检查handle输入，并执行方法。

        //如果当前对象不是自己，则将自己的位置信息平滑地传输到另外一个客户端
        //保证玩家在游玩时候移动不会出现卡顿
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

    #region 联网RPC方法

    /// <summary>
    /// 相机跟随方法
    /// </summary>
    [PunRPC]
    public void CameraFollow()
    {
        try
        {
            CinemachineVirtualCamera cinemachine = GameObject.Find("Camera").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            if (photonView.IsMine)
            {
                cinemachine.Follow = transform;//摄像机跟随玩家
            }
        }
        catch (Exception)
        {

            throw;
        }
        
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

    public void SetGravityScaleFun(float value, RpcTarget rpcTarget)
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
    /// 设置子物体活跃状态
    /// </summary>
    /// <param name="ifShouldActive"></param>
    [PunRPC]
    public void SetHandleEffectActive(bool ifShouldActive)
    {
        try
        {
            handleTF.gameObject.SetActive(ifShouldActive);
        }
        catch (Exception)
        {

        }
    }

    #endregion

    #region 检查方法
    public bool CheckIfIsMine()
    {
        return photonView.IsMine;
    }

    public bool CheckIfIsGrounded()
    {
        return Physics2D.OverlapCircle(
            checkGround.position,
            playerData.circleRadius,
            playerSelfData.whatIsGround);
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
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerSelfData.whatIsGround) && playerSelfData.ifHaveWallKey;
    }
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerSelfData.whatIsGround) && playerSelfData.ifHaveWallKey;
    }

    /// <summary>
    /// 检查是否拥有钥匙
    /// </summary>
    /// <returns></returns>
    public bool CheckIfIsHavingKey()
    {
        if (playerSelfData.ifHaveWallKey || playerSelfData.ifHaveDashKey || playerSelfData.ifHaveJumpKey)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 检查handle输入是否改变。
    /// </summary>
    public void CheckIfHandleInputChange()
    {
        if (preHandleInput != InputHandler.HandleInput)//当输入有变
        {
            if (InputHandler.HandleInput)//输入为true，则
            {
                photonView.RPC("SetHandleEffectActive", RpcTarget.Others, true);
                SetHandleEffectActiveNotRPC(true);
            }
            else//输入为false，则
            {
                photonView.RPC("SetHandleEffectActive", RpcTarget.Others, false);
                SetHandleEffectActiveNotRPC(false);
            }
        }
        preHandleInput = InputHandler.HandleInput;//将当前的输入赋值给上一帧的输入。
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
        playerSelfData.ifHaveJumpKey = true;
        playerSelfData.amountOfJump++;//增加最大跳跃次数
        JumpState.lastAmountOfJump++;//添加一次当前的临时跳跃次数
    }
    /// <summary>
    /// 设置触碰到闪避钥匙方法
    /// </summary>
    public void SetTouchingDashKey()
    {
        playerSelfData.ifHaveDashKey = true;
    }
    /// <summary>
    /// 设置触碰到爬墙钥匙方法
    /// </summary>
    public void SetTouchingWallKey()
    {
        playerSelfData.ifHaveWallKey = true;
    }
    /// <summary>
    /// 网络连接平滑方法
    /// </summary>
    public void Player2SmoothPostion()
    {
        transform.position = Vector3.Lerp(transform.position, posPlayerClone, 15.0f * Time.deltaTime);
    }
    /// <summary>
    /// 设置handle是否为活跃状态。
    /// </summary>
    /// <param name="ifShouldActive"></param>
    public void SetHandleEffectActiveNotRPC(bool ifShouldActive)
    {
        handleTF.gameObject.SetActive(ifShouldActive);
    }
    #endregion

    #region 其他方法
    /// <summary>
    /// 胜利方法
    /// </summary>
    public void WinGame()
    {
        RB.velocity = Vector3.zero;//将速度设置为0
        InputHandler.ControlHandler(false);//设置角色不可控制
        Invoke("ShowUI", 3);//三秒后显示UI
    }

    public void ShowUI()
    {
        //显示胜利UI
        UIManager.Instance.PushUI("UIWinGame");
    }
    
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
    public void resetLastAmountOfJump() => JumpState.lastAmountOfJump = playerSelfData.amountOfJump;

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
    /// <summary>
    /// 重新在保存点开始
    /// </summary>
    public void Restart()
    {
        photonView.RPC("RPCRestart", RpcTarget.All);
    }
    [PunRPC]
    public void RPCUIPop()
    {
        UIManager.Instance.PopUI();
    }
    [PunRPC]
    public void RPCRestart()
    {
        //////根据不同角色，分配不同位置，并且设置可以控制角色。
        //if (playerSelfData.selfIdentity == LayerMask.GetMask("PlayerDark"))
        //{
        //    transform.position = MainSceneRoot.Instance.DarkInitPos;
        //    InputHandler.ControlHandler(true);
        //}
        //else
        //{
        //    transform.position = MainSceneRoot.Instance.RedInitPos;
        //    InputHandler.ControlHandler(true);
        //}
        //重新加载本场景
        SceneMgr.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Dead()
    {
        photonView.RPC("RPCDead", RpcTarget.All);
    }
    /// <summary>
    /// 角色死亡方法。
    /// </summary>
    [PunRPC]
    public void RPCDead()
    {
        
        RB.velocity = Vector3.zero;//将速度设置为0
        InputHandler.ControlHandler(false);//设置角色不可控制
        isDead = true;//设置玩家死亡状态为true
        StartCoroutine(FadeAndAppear(true, 0.8f));//开启淡入淡出的协程
    }
    [PunRPC]
    public void Fade(float change)//控制显示隐藏的rpc
    {
        Debug.Log("执行消融");
        _myMaterial.SetFloat("_Fade", change);
    }

    //private void FadeAndAppear()
    //{
    //    if(isDead)
    //    {
    //        fadeTimer -= Time.deltaTime/playerData.fadeTime;
    //        photonView.RPC("Fade", RpcTarget.All, fadeTimer);
    //        if(fadeTimer <= -0.1f)
    //        {
    //            fadeTimer = -0.1f;
    //            isDead = true;//防止继续执行这段代码
    //        }
    //    }
    //}
    
    /// <summary>
    /// 淡入淡出方法
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeAndAppear(bool ifFade, float timerStartValue)
    {
        fadeTimer = timerStartValue;

        if (ifFade)
        {
            while (true)
            {
                fadeTimer -= Time.deltaTime / playerData.fadeTime;
                _myMaterial.SetFloat("_Fade", fadeTimer);
                if (fadeTimer <= -0.1f)
                {
                    fadeTimer = -0.1f;
                    UIManager.Instance.PushUI("UIEndGame");
                    yield break;
                }
                yield return -1;
            }
        }
        else
        {
            while (true)
            {
                fadeTimer += Time.deltaTime / playerData.fadeTime;
                _myMaterial.SetFloat("_Fade", fadeTimer);
                if (fadeTimer >= 0.8f)
                {
                    fadeTimer = 0.8f;
                    InputHandler.ControlHandler(true);//角色完全显示，才让玩家可以显示
                    yield break;
                }
                yield return -1;
            }
        }
    }

    public void playerDataClear()
    {
        playerSelfData.ifHaveDashKey = false;//初始化是否拥有闪避钥匙。
        playerSelfData.ifHaveWallKey = false;//初始化是否拥有爬墙技能。
        playerSelfData.ifHaveJumpKey = false;//初始化是否拥有连跳技能。
        preHandleInput = false;//初始化上一帧handle输入。
        playerSelfData.amountOfJump = 1;//初始化能够跳跃一次。
    }

    public void RPCPlayClip(string clipName, Vector3 position)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("PlayClipAtPoint", RpcTarget.Others, clipName, position);
        }
    }


    [PunRPC]
    public void PlayClipAtPoint(string clipName, Vector3 position)
    {
        MusicMgr.Instance.PlayAtPointFun(clipName, position, false);
    }
    #endregion
}
