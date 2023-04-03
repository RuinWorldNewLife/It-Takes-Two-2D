using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneData",menuName = "Data/SceneData")]
public class SceneData : ScriptableObject
{
    [Header("出生位置索引")]
    public int bornPosIndex;
}
    