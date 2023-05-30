using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class ButtonToWind : MonoBehaviour
{

    [SerializeField]
    private LayerMask playerLayer;

    Collider2D buttonTrigger;
    //使用哪一个计数器？
    [SerializeField]
    private int useWhatButtonCount = 1;

    [SerializeField]
    private float moveSpeed = 1.5f;//按钮移动速度

    [SerializeField]
    private Transform windBornTF;//生成风的位置

    [Header("生成竖直风还是水平风？")]
    [SerializeField]
    private string windToBorn = "winkHorizontal";

    Transform colliderTF;//子物体button的transform。

    Vector3 buttonStartPos;//button一开始的位置

    PhotonView photonView;
    private void Start()
    {
        buttonTrigger = GetComponent<Collider2D>();
        colliderTF = transform.GetChild(0);
        buttonStartPos = colliderTF.position;//初始化button开始的位置。

        photonView = GetComponent<PhotonView>();
        
        StaticData.buttonOneNum = 0;
        StaticData.buttonTwoNum = 0;
        StaticData.buttonThreeNum = 0;
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
                default:
                    break;
            }
            MusicMgr.Instance.PlayAtPointFun("chest_open", transform.position, false);
            //定时生成风的方法
            MonoHelper.Instance.InvokeReapeat(() =>
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject wind = PhotonNetwork.Instantiate(windToBorn, windBornTF.position, Quaternion.identity);
                }
            }, 4, () => { return false; });
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
            default:
                break;
        }
        //停止生成风的协程
        MonoHelper.Instance.InvokeReapeatStop();

        colliderTF.DOPause();
        colliderTF.DOKill();//在移动之前，先将之前的动画杀死
        //当物体离开按钮，则让按钮回到最初位置。
        colliderTF.DOMove(buttonStartPos, 0.2f);
    }
}
