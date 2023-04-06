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
            if (player == null) { return; }//���playerΪnull���򷵻�
            if (player.CheckIfIsHavingKey())
            {
                if (player.CheckIfIsMine() && !showedTip)//����Ƿ����Լ�
                {
                    UIManager.Instance.PushUI("UINoTip");
                    showedTip = true;
                }
                return;
            }//����Ƿ�ӵ��Կ�ף����ӵ�У��򷵻�
            player.SetTouchingJumpKey();
            sceneData.haveJumpKey = true;
            if (player.CheckIfIsMine())//����Ƿ����Լ�
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
