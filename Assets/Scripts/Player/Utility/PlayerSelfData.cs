using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/PlayerSelf Data")]
public class PlayerSelfData : ScriptableObject
{
    [Header("����")]
    public LayerMask whatIsGround;

    [Header("�Ƿ�����key")]
    public bool ifHaveWallKey;//�Ƿ�����ǽ����
    public bool ifHaveDashKey;//�Ƿ������ܼ��ܡ�
    public bool ifHaveJumpKey;

    public int amountOfJump;//��Ծ����
}
