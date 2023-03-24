using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 脚本功能：游戏入口 最先执行
/// </summary>
public class GameRoot : MonoBehaviour
{
    // 游戏的入口
    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
