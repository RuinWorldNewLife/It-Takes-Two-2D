using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 脚本功能：U控制器管理UIBase
/// </summary>
public class UIManager : Singleton<UIManager>
{
    private Stack<UIBase> uiStack;//负责管理UI的入栈和出栈
    private string uiPatDir = "UIPrefabs";
    private Dictionary<string, UIBase> uiBaseDic;

    
    private void Awake()
    {
        uiStack = new Stack<UIBase>();
        uiBaseDic = new Dictionary<string, UIBase>();
        //在场景跳转时不销毁UI管理器
        DontDestroyOnLoad(gameObject);
    }
    //控制UI入栈
    public void PushUI(string uiName)
    {
        //根据UIName 找到对应的UIBase
        //入栈
        UIBase uiBase = GetUIBase(uiName);

        if (uiStack.Count > 0)
        {
            //拿到旧的栈顶元素 并暂停 
            uiStack.Peek().DoOnPause();
        }
        //Debug.Log("进去的是"+uiBase);
        uiBase.DoOnStart();//开始激活新UI
        //让当前UI入栈
        uiStack.Push(uiBase);
    }

    //根据UIName 找到对应的UIBase
    private UIBase GetUIBase(string uiName) 
    {
        if (uiBaseDic.ContainsKey(uiName)) 
        {
            return uiBaseDic[uiName];
        }
        // 根据UI的名字 找到对应的预设体
        // 动态加载
        GameObject uiPrefab = Resources.Load<GameObject>(uiPatDir + "/" + uiName);
        GameObject uiObj = Instantiate(uiPrefab);
        //修改UI名字
        uiObj.name = uiName;
        UIBase uiBase = uiObj.GetComponent<UIBase>();
        uiBaseDic.Add(uiName, uiBase);//缓存起来
        return uiBase;
    }
    public void PopUI() 
    {
        //将当前的栈顶元素出栈并关闭
        uiStack.Pop().DoOnExit();
        if (uiStack.Count>0)
        {
            //Debug.Log(uiStack.Peek());
            //恢复栈顶元素
            uiStack.Peek().DoOnResume();
        }
    }
    //清空方法
    public void Clear()
    {
        uiStack.Clear();
        uiBaseDic.Clear();
    }
}
