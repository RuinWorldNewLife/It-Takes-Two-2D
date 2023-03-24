using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneObjColliderHandler : MonoBehaviour
{
    public UnityEvent onColliderHandler;
    /// <summary>
    /// 场景碰撞事件回调管理器
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        //当玩家碰撞到物体，执行event代码。
        if (other.gameObject.CompareTag("Player") && onColliderHandler != null)
        {
            onColliderHandler.Invoke();
        }
    }
}
