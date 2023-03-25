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

    #region һЩ����
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;
    private Vector3 posPlayerClone;
    private Transform handleTF;//handle������
    public bool IsMinePlayer { get;private set; }
    private bool updateInvokeAvoid;//������ִ��Э��

    private bool preHandleInput;//��һ֡�Ƿ��а���handle����

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
    public bool isJumpPlat;//�Ƿ�����������Ծ
    [HideInInspector]
    public float jumpPlatTime;
    [HideInInspector]
    public float jumpPlatForce;
    [HideInInspector]
    public float playerTempJumpSpeed;
    #endregion

    #region ״̬
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

    #region ���
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }

    [SerializeField]
    private Transform checkGround;
    //Photon��Ҫ��,PhotonView�ű�����
    private PhotonView photonView;
    [SerializeField]
    private Transform wallCheck;
    
    #endregion

    #region unity�Ļص�����
    private void Awake() 
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, playerSelfData, "In Air");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "In Air");
        wallHandleState = new WallHandleState(this, StateMachine, playerData, "wallHandle");
        wallSlideState = new WallSlide(this, StateMachine, playerData, "wallSlide");
        wallJumpState = new WallJumpState(this, StateMachine, playerData,playerSelfData, "In Air");
        RotateState = new PlayerRotateState(this, StateMachine, playerData, "Rotate");
        DashState = new PlayerDashState(this, StateMachine, playerData, playerSelfData, "Fly");
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
        handleTF = transform.GetChild(2);//�õ�������������HandlePos
        photonView.RPC("SetHandleEffectActive", RpcTarget.Others,false);
        SetHandleEffectActiveNotRPC(false);//���������������Handle��ʼ��Ϊfalse��
        cinemachine.Follow = transform;
        photonView.RPC("CameraFollow", RpcTarget.Others);///�������
        StateMachine.Initialize(IdleState);
        FacingDirection = 1;//��¼������
        isJumpPlat = false;
        isRotate = false;
        isInJumpOrDashState = false;
        updateInvokeAvoid = true;
        playerSelfData.ifHaveDashKey = false;//��ʼ���Ƿ�ӵ������Կ�ס�
        playerSelfData.ifHaveWallKey = false;//��ʼ���Ƿ�ӵ����ǽ���ܡ�
        playerSelfData.ifHaveJumpKey = false;//��ʼ���Ƿ�ӵ���������ܡ�
        preHandleInput = false;//��ʼ����һ֡handle���롣
        playerSelfData.amountOfJump = 1;//��ʼ���ܹ���Ծһ�Ρ�
        IsMinePlayer = photonView.IsMine;//�жϵ�ǰ��ɫ�Ƿ�Ϊ����
        RB.gravityScale = playerData.gravityValue;
        exitWindToFlyTime = playerData.exitWindToFlyTime;
        //��PhotonNetWork��ͨѶ���ʸ�Ϊÿ֡60��
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
    }
    

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
        if (isInJumpOrDashState && updateInvokeAvoid)
        {
            updateInvokeAvoid = false;
            MonoHelper.Instance.WaitSomeTimeInvoke(() => { }, 0, () => { return true; });//�����Э�̣�ֹͣЭ��
            MonoHelper.Instance.WaitSomeTimeInvoke(() => { isInJumpOrDashState = false; 
            updateInvokeAvoid = true;
            }, 0.2f, () => { return false; });
        }

        CheckIfHandleInputChange(); //���handle���룬��ִ�з�����

        //�����ǰ�������Լ������Լ���λ����Ϣƽ���ش��䵽����һ���ͻ���
        //��֤���������ʱ���ƶ�������ֿ���
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

    #region ����RPC����
    /// <summary>
    /// ������淽��
    /// </summary>
    [PunRPC]
    public void CameraFollow()
    {
        CinemachineVirtualCamera cinemachine = GameObject.Find("Camera").transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cinemachine.Follow = transform;
    }
    /// <summary>
    /// ������������
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
    /// �����Ƿ�����ʧЧ
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
    /// �����������Ծ״̬
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

    #region ��鷽��
    public bool CheckIfIsGrounded()
    {
        return Physics2D.OverlapCircle(
            checkGround.position,
            playerData.circleRadius,
            playerSelfData.whatIsGround) ;
    }
    /// <summary>
    /// Scene��ͼ��鷽��
    /// </summary>
    public void CheckIfShouldFlip(float xInput)
    {
        if (xInput != 0 && !(Mathf.Abs(xInput - FacingDirection) < 0.01f))
        {
            Flip();
        }
    }
    //�����Ƿ�����ǽ�ڡ�
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerSelfData.whatIsGround) && playerSelfData.ifHaveWallKey;
    }
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerSelfData.whatIsGround) && playerSelfData.ifHaveWallKey;
    }

    /// <summary>
    /// ����Ƿ�ӵ��Կ��
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
    /// ���handle�����Ƿ�ı䡣
    /// </summary>
    public void CheckIfHandleInputChange()
    {
        if(preHandleInput != InputHandler.HandleInput)//�������б�
        {
            if (InputHandler.HandleInput)//����Ϊtrue����
            {
                photonView.RPC("SetHandleEffectActive", RpcTarget.Others, true);
                SetHandleEffectActiveNotRPC(true);
            }
            else//����Ϊfalse����
            {
                photonView.RPC("SetHandleEffectActive", RpcTarget.Others, false);
                SetHandleEffectActiveNotRPC(false);
            }
        }
        preHandleInput = InputHandler.HandleInput;//����ǰ�����븳ֵ����һ֡�����롣
    }
    #endregion

    #region ���÷���
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
    /// ���ô�������ԾԿ�׵ķ�����
    /// </summary>
    public void SetTouchingJumpKey()
    {
        playerSelfData.ifHaveJumpKey = true;
        playerSelfData.amountOfJump++;//���������Ծ����
        JumpState.lastAmountOfJump++;//���һ�ε�ǰ����ʱ��Ծ����
    }
    /// <summary>
    /// ���ô���������Կ�׷���
    /// </summary>
    public void SetTouchingDashKey()
    {
        playerSelfData.ifHaveDashKey = true;
    }
    /// <summary>
    /// ���ô�������ǽԿ�׷���
    /// </summary>
    public void SetTouchingWallKey()
    {
        playerSelfData.ifHaveWallKey = true;
    }
    /// <summary>
    /// ��������ƽ������
    /// </summary>
    public void Player2SmoothPostion()
    {
        transform.position = Vector3.Lerp(transform.position, posPlayerClone, 15.0f * Time.deltaTime);
    }
    /// <summary>
    /// ����handle�Ƿ�Ϊ��Ծ״̬��
    /// </summary>
    /// <param name="ifShouldActive"></param>
    public void SetHandleEffectActiveNotRPC(bool ifShouldActive)
    {
        handleTF.gameObject.SetActive(ifShouldActive);
    }
    #endregion

    #region ��������
    /// <summary>
    /// ��ת����
    /// </summary>
    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(new Vector3(0, 180, 0));
    }
    /// <summary>
    /// ������Ծ��������
    /// </summary>
    public void resetLastAmountOfJump() => JumpState.lastAmountOfJump = playerSelfData.amountOfJump;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���ڴ����ʱ�򣬴����Լ���������Ϣ
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        //���ڶ�ȡ��ʱ�򣬽���ȡ������ϢǿתΪVector3�����ҽ��ա�
        if (stream.IsReading)
        {
            posPlayerClone = (Vector3)stream.ReceiveNext();
        }
    }
    #endregion
}
