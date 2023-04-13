
using UnityEngine;
[CreateAssetMenu(fileName = "newPlayerData",menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("������")]
    public LayerMask whatIsGround;
    public float circleRadius;//����������߰뾶
    [Header("վ��")]
    public float idle2WinkTime;
    [Header("����")]
    public float moveSpeed;
    [Header("��Ծ")]
    
    public float jumpSpeed;
    public float jumpHoldOnSpeed;
    public float jumpMinTime;
    public float stopyJumpVerlocity;
    
    [Header("����")]
    public float airMoveSpeed;
    public float jump2StopVelocity;
    [Header("ǽ��")]
    public float wallCheckDistance;
    public float variableJumpHeightMultiplier;
    public float WallSlideWaittingTime;
    public float wallSlideVelocity;
    public float wallJumpVelocity;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    public float wallJumpTime;
    public float wallHandleCD;
    
    [Header("����")]
    public float dashTime;
    
    [Header("�������")]
    public LayerMask WindLayer;
    public LayerMask windToFlyLayer;
    public float forceOfRidOfWind;
    public float gravityValue;
    public float dashSpeed;
    public float exitWindToFlyTime;
    [Header("����")]
    public float coyoteTime;
    public bool isTesting;
    public float fadeTime;//��ʧ�ٶ�
    
}
