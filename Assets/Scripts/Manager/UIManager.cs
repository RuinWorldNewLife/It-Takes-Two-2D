using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        
        if (uiStack.Count > 0)
        {
            //说明栈中有元素。
            //拿到旧的栈顶元素 并暂停 
            UIBase uIBase = uiStack.Peek();
            uIBase.DoOnPause();
            uIBase.gameObject.GetComponent<Canvas>().sortingOrder = 0;//将当前UI置于0层。

        }
        //根据UIName 找到对应的UIBase
        //入栈
        UIBase uiBase = GetUIBase(uiName);
        //让当前UI入栈
        uiStack.Push(uiBase);
        //Debug.Log("进去的是"+uiBase);
        uiBase.DoOnStart();//开始激活新UI
        if (uiStack.Count == 1) return;//如果当前只有栈底这一个UI，那就让它在最底部。
        uiBase.gameObject.GetComponent<Canvas>().sortingOrder = 1;//将当前UI置为最高层
    }

    /// <summary>
    /// 根据UIName 找到对应的UIBase
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    private UIBase GetUIBase(string uiName) 
    {
        if (uiBaseDic.ContainsKey(uiName)) 
        {
            return uiBaseDic[uiName];//如果字典中存在所需要的UI脚本，则直接将其返回
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
        if (uiStack.Count == 0) return;
        //将当前的栈顶元素出栈并关闭
        uiStack.Pop().DoOnExit();
        if (uiStack.Count>0)
        {
            //Debug.Log(uiStack.Peek());
            //恢复栈顶元素
            UIBase uiBase = uiStack.Peek();
            uiBase.DoOnResume();
        }

    }
    //清空方法
    public void Clear()
    {
        uiStack.Clear();
        uiBaseDic.Clear();
    }
}
