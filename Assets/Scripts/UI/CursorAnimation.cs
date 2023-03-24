using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 功能说明:鼠标动画
/// </summary>
public class CursorAnimation : MonoBehaviour
{
    //鼠标指针动画
    public Texture2D[] texture2Ds;
    private int cursourIndex;
    private float setCursorTimer;
    
    // private Vector3 targetPos;//目标位置
    // public GameObject clickEffectGo;

    /// <summary>
    /// 初始化指针
    /// </summary>
    private void Start()
    {
        Cursor.SetCursor(texture2Ds[cursourIndex],Vector2.zero,CursorMode.Auto);
    }

    private void Update()
    {
        UpdateCursor();
    }
    /// <summary>
    /// 更新指针动画
    /// </summary>
    public void UpdateCursor()
    {
        if (Time.time-setCursorTimer>= 0.1f)
        {
            cursourIndex++;
            if (cursourIndex >= texture2Ds.Length)
                cursourIndex = 0;
            
            Cursor.SetCursor(texture2Ds[cursourIndex],Vector2.zero,CursorMode.Auto);
            setCursorTimer = Time.time;
        }
    }
}
