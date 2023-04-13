using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExternalAffect : MonoBehaviour
{
    Player player;
    [SerializeField]
    PlayerData playerData;
    Collider2D playerCollider;

    float timer;
    bool startTimer;//是否开启递减。

    private void Start()
    {
        player = GetComponent<Player>();
        playerCollider = GetComponent<Collider2D>();
        startTimer = false;
        timer = 0.1f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            if (playerCollider.IsTouchingLayers(playerData.WindLayer))
            {
                player.isRotate = true;
            }
            if (playerCollider.IsTouchingLayers(playerData.windToFlyLayer))
            {
                timer = 0.1f;
                startTimer = true;
            }
        }
        catch (System.Exception)
        {
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
            timer = 0.1f;
            startTimer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == playerData.WindLayer.value)
        {
            player.isRotate = false;
        }
        //if ((1 << collision.gameObject.layer) == playerData.windToFlyLayer.value)
        //{
        //    startTimer = false;//如果执行了exit代码，则不要在update中进行计时操作了
        //    player.isFly = false;
        //    player.exitWindToFlyStartTime = Time.time;
        //    player.isInJumpOrDashState = false;
        //    if (player.StateMachine.CurrentState != player.DashState)
        //    {
        //        player.Anim.SetBool("Fly", false);
        //    }
        //}
    }
    private void Update()
    {

        if (startTimer)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0f)
        {
            Debug.Log("执行！！！");
            player.isFly = false;
            player.exitWindToFlyStartTime = Time.time;
            player.isInJumpOrDashState = false;
            if (player.StateMachine.CurrentState != player.DashState)
            {
                player.Anim.SetBool("Fly", false);
            }
            startTimer = false;
            timer = 0.1f;//初始化timer
        }
    }

}
