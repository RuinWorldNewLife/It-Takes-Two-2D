using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneObjColliderHandler : MonoBehaviour
{
    public UnityEvent onColliderHandler;
    /// <summary>
    /// ������ײ�¼��ص�������
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        //�������ײ�����壬ִ��event���롣
        if (other.gameObject.CompareTag("Player") && onColliderHandler != null)
        {
            onColliderHandler.Invoke();
        }
    }
}
