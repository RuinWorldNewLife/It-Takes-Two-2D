using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpPlat : MonoBehaviour
{
    public float jumpPlatTime;//在跳板上被施加力的时间，时间越长跳跃越高
    public float jumpPlatForce;//在跳板上跳跃的力
    [SerializeField]
    private LayerMask playerLayer;

    //[SerializeField]
    //private LayerMask darkPlayerLayer;
    private Collider2D platCollider;


    private PhotonView photonView;
    private Player player;


    private void Start()
    {
        platCollider = GetComponent<Collider2D>();
        photonView = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (platCollider.IsTouchingLayers(playerLayer))
        {
            player = collision.gameObject.GetComponent<Player>();
            MusicMgr.Instance.PlayAtPointFun("jump2", player.transform.position, false);
            player.jumpPlatTime = jumpPlatTime;
            player.isJumpPlat = true;
            player.jumpPlatForce = jumpPlatForce;
        }
    }
}
