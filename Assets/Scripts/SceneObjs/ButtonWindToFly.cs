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
    //ʹ����һ����������
    [SerializeField]
    private int useWhatButtonCount = 1;

    [SerializeField]
    private float moveSpeed = 1.5f;//��ť�ƶ��ٶ�

    Transform colliderTF;//������button��transform��

    Vector3 buttonStartPos;//buttonһ��ʼ��λ��

    PhotonView photonView;

    private Transform windToFlyTF;//����������������

    private void Start()
    {
        buttonTrigger = GetComponent<Collider2D>();
        colliderTF = transform.GetChild(0);
        buttonStartPos = colliderTF.position;//��ʼ��button��ʼ��λ�á�

        photonView = GetComponent<PhotonView>();

        StaticData.buttonOneNum = 0;
        StaticData.buttonTwoNum = 0;
        StaticData.buttonThreeNum = 0;

        windToFlyTF = transform.GetChild(1);//�õ�����������������
        photonView.RPC("SetWindToFlyActive", RpcTarget.Others, false);
        SetWindToFlyEffectActiveNotRPC(false);//��������������Ĺ���������setAcitve��ʼ��Ϊfalse��
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
            photonView.RPC("SetWindToFlyActive", RpcTarget.Others, true);
            SetWindToFlyEffectActiveNotRPC(true);//����Ұ�������ʱ������������
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
        photonView.RPC("SetWindToFlyActive", RpcTarget.Others, false);
        SetWindToFlyEffectActiveNotRPC(false);//������뿪��������������ʧ��
        colliderTF.DOPause();
        colliderTF.DOKill();//���ƶ�֮ǰ���Ƚ�֮ǰ�Ķ���ɱ��
        //�������뿪��ť�����ð�ť�ص����λ�á�
        colliderTF.DOMove(buttonStartPos, 0.2f);
    }
}
