using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAbilityKey : MonoBehaviour
{
    Collider2D keyCollider;
    public LayerMask playerLayer;
    private Player player;
    private void Start()
    {
        keyCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (keyCollider.IsTouchingLayers(playerLayer))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player == null) { return; }//如果player为null，则返回
            player.SetTouchingWallKey();
            if (player.CheckIfIsMine())//检查是否是自己
            {
                UIManager.Instance.PushUI("UIWallClimbAbility");
            }
            Destroy(gameObject, 0.2f);
        }
    }
}
