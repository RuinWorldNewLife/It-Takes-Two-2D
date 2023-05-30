using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DashAbilityKey : MonoBehaviour
{
    Collider2D keyCollider;
    public LayerMask playerLayer;
    private Player player;
    [SerializeField]
    private SceneData sceneData;
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
            player.SetTouchingDashKey();
            MusicMgr.Instance.PlayAtPointFun("spell_pickup_final_boom", player.transform.position, false);
            sceneData.haveDashKey = true;
            if (player.CheckIfIsMine())//检查是否是自己
            {
                UIManager.Instance.PushUI("UIDashAbility");
            }
            Invoke("DestoryDashKey", 0.2f);
        }
    }
    public void DestoryDashKey()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }
}
