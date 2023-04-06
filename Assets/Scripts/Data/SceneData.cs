using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneData",menuName = "Data/SceneData")]
public class SceneData : ScriptableObject
{
    [Header("出生位置索引")]
    public int bornPosIndex;
    [Header("闪避是否被获得")]
    public bool haveDashKey;
    [Header("跳跃是否被获得")]
    public bool haveJumpKey;
    [Header("爬墙是否被获得")]
    public bool haveWallClimbKey;
}
    