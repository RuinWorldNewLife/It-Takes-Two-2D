using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/PlayerSelf Data")]
public class PlayerSelfData : ScriptableObject
{
    [Header("地面")]
    public LayerMask whatIsGround;

    [Header("是否碰到key")]
    public bool ifHaveWallKey;//是否有爬墙技能
    public bool ifHaveDashKey;//是否有闪避技能。
    public bool ifHaveJumpKey;

    public int amountOfJump;//跳跃次数
}
