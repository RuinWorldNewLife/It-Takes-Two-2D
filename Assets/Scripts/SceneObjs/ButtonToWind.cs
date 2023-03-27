using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class ButtonToWind : MonoBehaviour
{
    //���ɷ�ķ���
    [SerializeField]
    private Vector2 windDirection;

    [SerializeField]
    private LayerMask playerLayer;

    Collider2D buttonTrigger;
    //ʹ����һ����������
    [SerializeField]
    private int useWhatButtonCount = 1;

    [SerializeField]
    private float moveSpeed = 1.5f;//��ť�ƶ��ٶ�

    [SerializeField]
    private Vector3 windBornPos;//���ɷ��λ��

    Transform colliderTF;//������button��transform��

    Vector3 buttonStartPos;//buttonһ��ʼ��λ��
    private void Start()
    {
        buttonTrigger = GetComponent<Collider2D>();
        colliderTF = transform.GetChild(0);
        buttonStartPos = colliderTF.position;//��ʼ��button��ʼ��λ�á�
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
                default:
                    break;
            }
            //��ʱ���ɷ�ķ���
            MonoHelper.Instance.InvokeReapeat(() =>
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject wind = PhotonNetwork.Instantiate("wink", windBornPos, Quaternion.identity);
                    wind.GetComponent<Wind>().windDirection = windDirection;
                }
            }, 6, () => { return false; });
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (buttonTrigger.IsTouchingLayers(playerLayer))
        {
            
            if (colliderTF.position.y >= buttonStartPos.y - 0.1f)
            {
                //����ť�����ƶ�
                colliderTF.Translate(Vector3.down * moveSpeed / 2 * Time.deltaTime);
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
            default:
                break;
        }
        //ֹͣ���ɷ��Э��
        MonoHelper.Instance.InvokeReapeatStop();

        colliderTF.DOPause();
        colliderTF.DOKill();//���ƶ�֮ǰ���Ƚ�֮ǰ�Ķ���ɱ��
        //�������뿪��ť�����ð�ť�ص����λ�á�
        colliderTF.DOMove(buttonStartPos, 0.2f);
    }
}
