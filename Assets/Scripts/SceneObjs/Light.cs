using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public bool isOn = false;//设置初始是否为开启
    [Header("场景数据")]
    [SerializeField]
    private SceneData sceneData;//场景数据
    public int indexOfBornPos;//设置出生索引值，这个值用来设定
    public LayerMask playerLayer;

    private Collider2D lightCollider;
    private GameObject LightOff;
    private GameObject LightOn;//拿到两个灯子物体
    private GameObject _2DLight;//拿到点光源

    private void Start()
    {
        lightCollider = GetComponent<Collider2D>();
        LightOff = transform.GetChild(0).gameObject;
        LightOn = transform.GetChild(1).gameObject;//拿到两个灯子物体
        _2DLight = transform.GetChild(2).gameObject;//拿到点光源
        LightOff.SetActive(!isOn);
        LightOn.SetActive(isOn);//根据初始值，设定路灯是开启或者关闭
        _2DLight.SetActive(isOn);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(lightCollider.IsTouchingLayers(playerLayer))
        {
            sceneData.bornPosIndex = indexOfBornPos;//调整出生索引值。
            MainSceneRoot.Instance.SetBornPos();//重新设置出生位置
            isOn = true;
            LightOff.SetActive(!isOn);
            LightOn.SetActive(isOn);//设定路灯是开启或者关闭
            _2DLight.SetActive(isOn);
        }
    }
}
