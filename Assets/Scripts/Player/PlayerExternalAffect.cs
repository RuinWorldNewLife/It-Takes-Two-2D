using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExternalAffect : MonoBehaviour
{
    Player player;
    [SerializeField]
    PlayerData playerData;
    Collider2D playerCollider;
    private void Start()
    {
        player = GetComponent<Player>();
        playerCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerCollider.IsTouchingLayers(playerData.WindLayer))
        {
            player.isRotate = true;
        }
        if (playerCollider.IsTouchingLayers(playerData.windToFlyLayer))
        {
            player.isFly = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerCollider.IsTouchingLayers(playerData.WindLayer))
        {
            player.isRotate = true;
        }
        if (playerCollider.IsTouchingLayers(playerData.windToFlyLayer))
        {
            player.isFly = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == playerData.WindLayer.value)
        {
            player.isRotate = false;
        }
        if((1 << collision.gameObject.layer) == playerData.windToFlyLayer)
        {
            player.isFly = false;
            player.exitWindToFlyStartTime = Time.time;
        }
    }
}
