using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class ButtonWindToFly : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    Collider2D buttonTrigger;
    //使用哪一个计数器？
    [SerializeField]
    private int useWhatButtonCount = 1;

    [SerializeField]
    private float moveSpeed = 1.5f;//按钮移动速度

    Transform colliderTF;//子物体button的transform。

    Vector3 buttonStartPos;//button一开始的位置

    PhotonView photonView;

    private Transform windToFlyTF;//光柱子物体子物体

    private void Start()
    {
        buttonTrigger = GetComponent<Collider2D>();
        colliderTF = transform.GetChild(0);
        buttonStartPos = colliderTF.position;//初始化button开始的位置。

        photonView = GetComponent<PhotonView>();

        StaticData.buttonOneNum = 0;
        StaticData.buttonTwoNum = 0;
        StaticData.buttonThreeNum = 0;
        StaticData.buttonFourNum = 0;
        StaticData.buttonFiveNum = 0;
        StaticData.buttonSixNum = 0;

        windToFlyTF = transform.GetChild(1);//拿到第三个光柱子物体
        photonView.RPC("SetWindToFlyActive", RpcTarget.Others, false);
        SetWindToFlyEffectActiveNotRPC(false);//将第三个子物体的光柱子物体setAcitve初始化为false。
    }

    [PunRPC]
    public void SetWindToFlyActive(bool ifShouldActive)
    {
        windToFlyTF.gameObject.SetActive(ifShouldActive);
    }

    public void SetWindToFlyEffectActiveNotRPC(bool ifShouldActive)
    {
        windToFlyTF.gameObject.SetActive(ifShouldActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (buttonTrigger.IsTouchingLayers(playerLayer))
        {
            switch (useWhatButtonCount)
            {
                case 1:
                    StaticData.buttonOneNum++;//每次有玩家站在平台上，让按钮一上的玩家数量自增。
                    //if (StaticData.buttonOneNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                    break;
                case 2:
                    StaticData.buttonTwoNum++;//每次有玩家站在平台上，让按钮一上的玩家数量自增。
                    //if (StaticData.buttonTwoNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                    break;
                case 3:
                    StaticData.buttonThreeNum++;//每次有玩家站在平台上，让按钮一上的玩家数量自增。
                    //if (StaticData.buttonThreeNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                    break;
                case 4:
                    StaticData.buttonFourNum++;//每次有玩家站在平台上，让按钮一上的玩家数量自增。
                    //if (StaticData.buttonThreeNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                    break;
                case 5:
                    StaticData.buttonFiveNum++;//每次有玩家站在平台上，让按钮一上的玩家数量自增。
                    //if (StaticData.buttonThreeNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                    break;
                case 6:
                    StaticData.buttonSixNum++;//每次有玩家站在平台上，让按钮一上的玩家数量自增。
                    //if (StaticData.buttonThreeNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                    break;
                default:
                    break;
            }
            photonView.RPC("SetWindToFlyActive", RpcTarget.Others, true);
            SetWindToFlyEffectActiveNotRPC(true);//当玩家按到按键时，将光柱生成
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (buttonTrigger.IsTouchingLayers(playerLayer))
        {

            if (colliderTF.position.y >= buttonStartPos.y - 0.1f)
            {
                //本身按钮向下移动
                colliderTF.Translate(Vector3.down * moveSpeed / 2 * Time.deltaTime);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //当物体离开按钮，让按钮上的玩家数量自减。
        switch (useWhatButtonCount)
        {
            case 1:
                StaticData.buttonOneNum--;
                if (StaticData.buttonOneNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                break;
            case 2:
                StaticData.buttonTwoNum--;
                if (StaticData.buttonTwoNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                break;
            case 3:
                StaticData.buttonThreeNum--;
                if (StaticData.buttonThreeNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                break;
            case 4:
                StaticData.buttonFourNum--;
                if (StaticData.buttonFourNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                break;
            case 5:
                StaticData.buttonFiveNum--;
                if (StaticData.buttonFiveNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                break;
            case 6:
                StaticData.buttonSixNum--;
                if (StaticData.buttonSixNum > 0) return;//如果按钮上的玩家数量大于0，则返回。
                break;
            default:
                break;
        }
        photonView.RPC("SetWindToFlyActive", RpcTarget.Others, false);
        MusicMgr.Instance.PlayAtPointFun("chest_open", transform.position, false);

        SetWindToFlyEffectActiveNotRPC(false);//当玩家离开按键，将光柱消失。
        colliderTF.DOPause();
        colliderTF.DOKill();//在移动之前，先将之前的动画杀死
        //当物体离开按钮，则让按钮回到最初位置。
        colliderTF.DOMove(buttonStartPos, 0.2f);
    }
}
