using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    private void Start()
    {
		UIManager.Instance.PushUI("UIGameStart");//��ѡ����ʾ
        //Debug.Log("������ҳ��");
    }
}
