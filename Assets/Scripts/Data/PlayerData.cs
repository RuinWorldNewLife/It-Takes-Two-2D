
using UnityEngine;
[CreateAssetMenu(fileName = "newPlayerData",menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("检测变量")]
    public LayerMask whatIsGround;
    public float circleRadius;//检测地面的射线半径
    [Header("站立")]
    public float idle2WinkTime;
    [Header("步行")]
    public float moveSpeed;
    [Header("跳跃")]
    
    public float jumpSpeed;
    public float jumpHoldOnSpeed;
    public float jumpMinTime;
    public float stopyJumpVerlocity;
    
    [Header("空中")]
    public float airMoveSpeed;
    public float jump2StopVelocity;
    [Header("墙上")]
    public float wallCheckDistance;
    public float variableJumpHeightMultiplier;
    public float WallSlideWaittingTime;
    public float wallSlideVelocity;
    public float wallJumpVelocity;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    public float wallJumpTime;
    public float wallHandleCD;
    
    [Header("闪避")]
    public float dashTime;
    
    [Header("外界因素")]
    public LayerMask WindLayer;
    public LayerMask windToFlyLayer;
    public float forceOfRidOfWind;
    public float gravityValue;
    public float dashSpeed;
    public float exitWindToFlyTime;
    [Header("其他")]
    public float coyoteTime;
    public bool isTesting;
    public float fadeTime;//消失速度
    
}
