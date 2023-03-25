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
            if (player.CheckIfIsHavingKey())
            {
                return;
            }//检查是否拥有钥匙，如果拥有，则返回
            player.SetTouchingWallKey();
            Destroy(gameObject, 0.2f);
        }
    }
}
