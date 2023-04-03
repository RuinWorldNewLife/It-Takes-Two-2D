using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public bool isOn = false;//���ó�ʼ�Ƿ�Ϊ����
    [Header("��������")]
    [SerializeField]
    private SceneData sceneData;//��������
    public int indexOfBornPos;//���ó�������ֵ�����ֵ�����趨
    public LayerMask playerLayer;

    private Collider2D lightCollider;
    private GameObject LightOff;
    private GameObject LightOn;//�õ�������������

    private void Start()
    {
        lightCollider = GetComponent<Collider2D>();
        LightOff = transform.GetChild(0).gameObject;
        LightOn = transform.GetChild(1).gameObject;//�õ�������������

        LightOff.SetActive(!isOn);
        LightOn.SetActive(isOn);//���ݳ�ʼֵ���趨·���ǿ������߹ر�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(lightCollider.IsTouchingLayers(playerLayer))
        {
            sceneData.bornPosIndex = indexOfBornPos;//������������ֵ��
            MainSceneRoot.Instance.SetBornPos();//�������ó���λ��
            isOn = true;
            LightOff.SetActive(!isOn);
            LightOn.SetActive(isOn);//�趨·���ǿ������߹ر�
        }
    }
}
