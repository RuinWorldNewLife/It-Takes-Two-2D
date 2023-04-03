using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToDead : MonoBehaviour
{

    CompositeCollider2D attack;
    
    public LayerMask playerLayer;
    private void Start()
    {
        attack = GetComponent<CompositeCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (attack.IsTouchingLayers(playerLayer))
        {
            //Debug.Log("������");
            //��������Һ󣬵�����������
            collision.gameObject.GetComponent<Player>().Dead();
        }
    }
}
