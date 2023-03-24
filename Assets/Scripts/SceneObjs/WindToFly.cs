using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindToFly : MonoBehaviour
{
    private float yVerlocity;
    Collider2D windCollider;
    //[SerializeField]
    //private PlayerData playerData;
    public Vector2 facingDirection;
    [SerializeField]
    private float toMidSpeed;
    [SerializeField]
    private float flySpeed;
    [SerializeField]
    private LayerMask affectObjLayers;

    
    //[SerializeField]
    //private LayerMask affectObjLayersForDark;


    private Player player;


    private void Start()
    {
        windCollider = GetComponent<Collider2D>();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (windCollider.IsTouchingLayers(affectObjLayers))
        {
            player = collision.gameObject.GetComponent<Player>();
            player.windToFlyDirection = facingDirection;
            if (facingDirection.x > 0)
            {
                if (player.IsMinePlayer)
                {
                    player.Anim.SetBool("Fly", true);
                }
            }
            if (facingDirection.y > 0)
            {
                if (player.IsMinePlayer)
                {
                    player.Anim.SetBool("In Air", true);
                }
            }
        }
        //if (windCollider.IsTouchingLayers(affectObjLayersForDark))
        //{
        //    darkPlayer.windToFlyDirection = facingDirection;
        //    if (facingDirection.x > 0)
        //    {
        //        darkPlayer.Anim.SetBool("Fly", true);
        //    }
        //    if (facingDirection.y > 0)
        //    {
        //        darkPlayer.Anim.SetBool("In Air", true);
        //    }
        //}

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (windCollider.IsTouchingLayers(affectObjLayers))
        {
            player = collision.gameObject.GetComponent<Player>();
            yVerlocity = player.CurrentVelocity.normalized.y;

            if (facingDirection.x != 0)
            {
                if (player.FacingDirection != player.windToFlyDirection.x)
                {
                    player.Flip();
                }
                if (!player.isInJumpOrDashState)
                {
                    player.SetVelocityX(facingDirection.x * flySpeed);
                    player.SetVelocityY((gameObject.transform.position.y - player.transform.position.y) * toMidSpeed);
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetBool("In Air", false);
                        player.Anim.SetBool("Fly", true);
                    }
                }
                else if (player.StateMachine.CurrentState != player.DashState)
                {
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetBool("Fly", false);
                        player.Anim.SetBool("In Air", true);
                    }
                }
            }
            else if (facingDirection.y != 0)
            {
                player.CheckIfShouldFlip(player.InputHandler.NormInputX);

                if (!player.isInJumpOrDashState)
                {
                    player.SetVelocityY(facingDirection.y * flySpeed);
                    player.SetVelocityX((gameObject.transform.position.x - player.transform.position.x) * toMidSpeed);
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetBool("In Air", true);
                        player.Anim.SetFloat("yVerlocity", yVerlocity);
                    }
                }
                else if (player.StateMachine.CurrentState == player.DashState)
                {
                    if (player.IsMinePlayer)
                    {
                        player.Anim.SetBool("Fly", true);
                        player.Anim.SetBool("In Air", false);
                    }
                }
            }
        }
        //if (windCollider.IsTouchingLayers(affectObjLayersForDark))
        //{
        //    yVerlocity = darkPlayer.CurrentVelocity.normalized.y;
        //    if (facingDirection.x != 0)
        //    {
        //        if (darkPlayer.FacingDirection != darkPlayer.windToFlyDirection.x)
        //        {
        //            darkPlayer.Flip();
        //        }
        //        if (!darkPlayer.isInJumpOrDashState)
        //        {
        //            darkPlayer.Anim.SetBool("In Air", false);
        //            darkPlayer.Anim.SetBool("Fly", true);
        //            darkPlayer.SetVelocityX(facingDirection.x * flySpeed);
        //            darkPlayer.SetVelocityY((gameObject.transform.position.y - darkPlayer.transform.position.y) * toMidSpeed);
        //        }
        //        else if (darkPlayer.StateMachine.CurrentState != darkPlayer.DashState)
        //        {
        //            darkPlayer.Anim.SetBool("Fly", false);
        //            darkPlayer.Anim.SetBool("In Air", true);
        //        }
        //    }
        //    else if (facingDirection.y != 0)
        //    {
        //        darkPlayer.CheckIfShouldFlip(darkPlayer.InputHandler.NormInputX);

        //        if (!darkPlayer.isInJumpOrDashState)
        //        {
        //            darkPlayer.Anim.SetBool("In Air", true);
        //            darkPlayer.SetVelocityY(facingDirection.y * flySpeed);
        //            darkPlayer.SetVelocityX((gameObject.transform.position.x - darkPlayer.transform.position.x) * toMidSpeed);
        //            darkPlayer.Anim.SetFloat("yVerlocity", yVerlocity);
        //        }
        //        else if (darkPlayer.StateMachine.CurrentState == darkPlayer.DashState)
        //        {
        //            darkPlayer.Anim.SetBool("Fly", true);
        //            darkPlayer.Anim.SetBool("In Air", false);
        //        }
        //    }
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == affectObjLayers.value)
        {
            player.isInJumpOrDashState = false;
            if (player.StateMachine.CurrentState != player.DashState)
            {
                player.Anim.SetBool("Fly", false);
            }
        }
        //if ((1 << collision.gameObject.layer) == affectObjLayersForDark.value)
        //{
        //    darkPlayer.isInJumpOrDashState = false;
        //    if (darkPlayer.StateMachine.CurrentState != darkPlayer.DashState)
        //    {
        //        darkPlayer.Anim.SetBool("Fly", false);
        //    }
        //}
    }
}
