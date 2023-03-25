using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class HandleEvent : MonoBehaviour
{
    Player player;

    [SerializeField]
    private LayerMask keyLayer;
    [SerializeField]
    private float absorbSpeed = 0.05f;

    private Collider2D handleCollider;
    private void Awake()
    {
        player = transform.GetParentComponent<Player>();
        handleCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (player == null) { return; }//���playerΪnull���򷵻�
        //�����ײ����Կ�ף���Կ��������Ȧ�С�
        if (handleCollider.IsTouchingLayers(keyLayer) && player.CheckIfIsHavingKey() && (1 << collision.gameObject.layer) == keyLayer.value)
        {
            collision.transform.DOMove(transform.position, absorbSpeed);
        }
        
    }
}
