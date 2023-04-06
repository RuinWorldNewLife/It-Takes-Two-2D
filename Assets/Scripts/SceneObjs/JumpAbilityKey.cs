using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbilityKey : MonoBehaviour
{
    Collider2D keyCollider;
    public LayerMask playerLayer;
    private Player player;
    [SerializeField]
    private SceneData sceneData;
    private bool showedTip;
    private void Start()
    {
        keyCollider = GetComponent<Collider2D>();
        showedTip = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (keyCollider.IsTouchingLayers(playerLayer))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player == null) { return; }//如果player为null，则返回
            if (player.CheckIfIsHavingKey())
            {
                if (player.CheckIfIsMine() && !showedTip)//检查是否是自己
                {
                    UIManager.Instance.PushUI("UINoTip");
                    showedTip = true;
                }
                return;
            }//检查是否拥有钥匙，如果拥有，则返回
            player.SetTouchingJumpKey();
            sceneData.haveJumpKey = true;
            if (player.CheckIfIsMine())//检查是否是自己
            {
                UIManager.Instance.PushUI("UIDoubleJumpAbility");
            }
            Invoke("DestoryJumpKey", 0.2f);
        }

    }
    public void DestoryJumpKey()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }
}
