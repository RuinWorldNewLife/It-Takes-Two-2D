using Photon.Realtime;
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
            //Debug.Log("触碰到");
            MusicMgr.Instance.PlayAtPointFun("hero_damage", collision.transform.position, false);
            //触碰到玩家后，调用死亡代码
            collision.gameObject.GetComponent<Player>().Dead();
        }
    }
}
