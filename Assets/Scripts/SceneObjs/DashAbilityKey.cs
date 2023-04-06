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
            if (player == null) { return; }//���playerΪnull���򷵻�
            if (player.CheckIfIsHavingKey())
            {
                return;
            }//����Ƿ�ӵ��Կ�ף����ӵ�У��򷵻�
            player.SetTouchingDashKey();
            sceneData.haveDashKey = true;
            if (player.CheckIfIsMine())//����Ƿ����Լ�
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
