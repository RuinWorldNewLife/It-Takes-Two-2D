using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MobilePlat : MonoBehaviour
{

    [SerializeField]
    private LayerMask affectObjLayers;//���԰��°�ť����Ʒ��
    [Header("ƽ̨�ƶ��ٶ�")]
    [SerializeField]
    private float moveSpeed;//ƽ̨�ƶ��ٶ�
    [Header("ƽ̨�ƶ���ʱ��")]
    [SerializeField]
    private float timeToMovePlat = 3f;//ƽ̨�ص�ԭλ��ʱ�䡣
    [Header("ʹ����һ������������")]
    [SerializeField]
    private int useWhatButtonCount = 1;

    Collider2D myCollider;

    Transform platTransform;

    Transform startTF;//�ʼ��λ��

    Transform endTF;//����λ��

    Vector3 buttonStartPos;//��ť�ĳ�ʼλ�á�

    Transform colliderBox;//������ײ�е�������TF

    bool trueUpFalseDown;//�ж��ƶ�ƽ̨��Ӧ�����������½�

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        platTransform = transform.parent.GetChild(0).transform;
        colliderBox = transform.GetChild(2);
        buttonStartPos = colliderBox.position;//��ʼ����ť��ʼλ��
        startTF = transform.GetChild(0);
        endTF = transform.GetChild(1);
        //��ʼ���ж��ƶ�ƽ̨���������½���
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
        //����������ɫ
        if (myCollider.IsTouchingLayers(affectObjLayers))
        {
            switch (useWhatButtonCount)
            {
                case 1:
                    StaticData.buttonOneNum++;//ÿ�������վ��ƽ̨�ϣ��ð�ťһ�ϵ��������������
                    //if (StaticData.buttonOneNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                    break;
                case 2:
                    StaticData.buttonTwoNum++;//ÿ�������վ��ƽ̨�ϣ��ð�ťһ�ϵ��������������
                    //if (StaticData.buttonTwoNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                    break;
                case 3:
                    StaticData.buttonThreeNum++;//ÿ�������վ��ƽ̨�ϣ��ð�ťһ�ϵ��������������
                    //if (StaticData.buttonThreeNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                    break;
                case 4:
                    StaticData.buttonFourNum++;//ÿ�������վ��ƽ̨�ϣ��ð�ťһ�ϵ��������������
                    //if (StaticData.buttonThreeNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                    break;
                case 5:
                    StaticData.buttonFiveNum++;//ÿ�������վ��ƽ̨�ϣ��ð�ťһ�ϵ��������������
                    //if (StaticData.buttonThreeNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                    break;
                case 6:
                    StaticData.buttonSixNum++;//ÿ�������վ��ƽ̨�ϣ��ð�ťһ�ϵ��������������
                    //if (StaticData.buttonThreeNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                    break;
                default:
                    break;
            }
            colliderBox.DOPause();
            platTransform.DOPause();
            colliderBox.DOKill();
            platTransform.DOKill();//���ƶ�֮ǰ���Ƚ�֮ǰ�Ķ���ɱ��
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //����������ɫ
        if (myCollider.IsTouchingLayers(affectObjLayers))
        {
            if (colliderBox.position.y >= buttonStartPos.y - 0.1f)
            {
                //����ť�����ƶ�
                colliderBox.Translate(Vector3.down * moveSpeed / 2 * Time.deltaTime);
            }
            else//����ť�Ѿ������ף�ִ��ƽ̨�ƶ�����
            {
                //�ж�ƽ̨Ӧ�����������½���trueΪ������flaseΪ�½���
                if (trueUpFalseDown)
                {
                    if (platTransform.position.y >= endTF.position.y) return;//���ƽ̨�߶ȴﵽ��ߣ��򷵻�
                                                                             //ƽ̨������
                    platTransform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                }
                else
                {
                    if (platTransform.position.y <= endTF.position.y) return;//���ƽ̨�߶ȴﵽ��ͣ��򷵻�
                                                                             //ƽ̨���½�
                    platTransform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
                }

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�������뿪��ť���ð�ť�ϵ���������Լ���
        switch (useWhatButtonCount)
        {
            case 1:
                StaticData.buttonOneNum--;
                if (StaticData.buttonOneNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                break;
            case 2:
                StaticData.buttonTwoNum--;
                if (StaticData.buttonTwoNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                break;
            case 3:
                StaticData.buttonThreeNum--;
                if (StaticData.buttonThreeNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                break;
            case 4:
                StaticData.buttonFourNum--;
                if (StaticData.buttonFourNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                break;
            case 5:
                StaticData.buttonFiveNum--;
                if (StaticData.buttonFiveNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                break;
            case 6:
                StaticData.buttonSixNum--;
                if (StaticData.buttonSixNum > 0) return;//�����ť�ϵ������������0���򷵻ء�
                break;
            default:
                break;
        }
        colliderBox.DOPause();
        platTransform.DOPause();
        colliderBox.DOKill();
        platTransform.DOKill();//���ƶ�֮ǰ���Ƚ�֮ǰ�Ķ���ɱ��
        //�������뿪��ť�������ƶ�ƽ̨�ص����λ�á�
        platTransform.DOMove(startTF.position, timeToMovePlat);
        //�������뿪��ť�����ð�ť�ص����λ�á�
        colliderBox.DOMove(buttonStartPos, 0.2f);
    }
}
