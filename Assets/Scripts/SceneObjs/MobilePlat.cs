using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MobilePlat : MonoBehaviour
{

    [SerializeField]
    private LayerMask affectObjLayers;//可以按下按钮的物品层
    [Header("平台移动速度")]
    [SerializeField]
    private float moveSpeed;//平台移动速度
    [Header("平台移动的时间")]
    [SerializeField]
    private float timeToMovePlat = 3f;//平台回到原位的时间。
    [Header("使用哪一个数量计数器")]
    [SerializeField]
    private int useWhatButtonCount = 1;

    Collider2D myCollider;

    Transform platTransform;

    Transform startTF;//最开始的位置

    Transform endTF;//最终位置

    Vector3 buttonStartPos;//按钮的初始位置。

    Transform colliderBox;//承载碰撞盒的子物体TF

    bool trueUpFalseDown;//判断移动平台是应该上升还是下降

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        platTransform = transform.parent.GetChild(0).transform;
        colliderBox = transform.GetChild(2);
        buttonStartPos = colliderBox.position;//初始化按钮初始位置
        startTF = transform.GetChild(0);
        endTF = transform.GetChild(1);
        //初始化判断移动平台上升或者下降。
        if (startTF.position.y < endTF.position.y)
        {
            trueUpFalseDown = true;
        }
        else
        {
            trueUpFalseDown = false;
        }
        StaticData.buttonOneNum = 0;
        StaticData.buttonTwoNum = 0;
        StaticData.buttonThreeNum = 0;
        StaticData.buttonFourNum = 0;
        StaticData.buttonFiveNum = 0;
        StaticData.buttonSixNum = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当触碰到角色
        if (myCollider.IsTouchingLayers(affectObjLayers))
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
            colliderBox.DOPause();
            platTransform.DOPause();
            colliderBox.DOKill();
            platTransform.DOKill();//在移动之前，先将之前的动画杀死
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //当触碰到角色
        if (myCollider.IsTouchingLayers(affectObjLayers))
        {
            if (colliderBox.position.y >= buttonStartPos.y - 0.1f)
            {
                //本身按钮向下移动
                colliderBox.Translate(Vector3.down * moveSpeed / 2 * Time.deltaTime);
            }
            else//当按钮已经按到底，执行平台移动方法
            {
                //判断平台应该上升还是下降，true为上升，flase为下降。
                if (trueUpFalseDown)
                {
                    if (platTransform.position.y >= endTF.position.y) return;//如果平台高度达到最高，则返回
                                                                             //平台向上升
                    platTransform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                }
                else
                {
                    if (platTransform.position.y <= endTF.position.y) return;//如果平台高度达到最低，则返回
                                                                             //平台向下降
                    platTransform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                }

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
        colliderBox.DOPause();
        platTransform.DOPause();
        colliderBox.DOKill();
        platTransform.DOKill();//在移动之前，先将之前的动画杀死
        //当物体离开按钮，则让移动平台回到最初位置。
        platTransform.DOMove(startTF.position, timeToMovePlat);
        //当物体离开按钮，则让按钮回到最初位置。
        colliderBox.DOMove(buttonStartPos, 0.2f);
    }
}
