using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    private void Start()
    {
		UIManager.Instance.PushUI("UIGameStart");//将选项显示
        //Debug.Log("加载新页面");
    }
}
