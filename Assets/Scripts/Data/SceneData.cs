using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneData",menuName = "Data/SceneData")]
public class SceneData : ScriptableObject
{
    [Header("����λ������")]
    public int bornPosIndex;
    [Header("�����Ƿ񱻻��")]
    public bool haveDashKey;
    [Header("��Ծ�Ƿ񱻻��")]
    public bool haveJumpKey;
    [Header("��ǽ�Ƿ񱻻��")]
    public bool haveWallClimbKey;
}
    