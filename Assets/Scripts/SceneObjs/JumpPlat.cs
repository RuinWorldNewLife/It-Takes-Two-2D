using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumpPlat : MonoBehaviour
{
    public float jumpPlatTime;//�������ϱ�ʩ������ʱ�䣬ʱ��Խ����ԾԽ��
    public float jumpPlatForce;//����������Ծ����
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
            player.jumpPlatTime = jumpPlatTime;
            player.isJumpPlat = true;
            player.jumpPlatForce = jumpPlatForce;
        }
    }
}
