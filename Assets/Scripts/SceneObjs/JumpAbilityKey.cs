using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbilityKey : MonoBehaviour
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
            player.SetTouchingJumpKey();
            Destroy(gameObject, 0.2f);

        }
    }
}
