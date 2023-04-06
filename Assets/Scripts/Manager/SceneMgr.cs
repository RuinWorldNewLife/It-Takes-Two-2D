using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 脚本功能: 控制场景跳转
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{

    private void Start()
    {
        //当场景加载时 绑定事件
        //Unity事件两个参数无返回值
        SceneManager.sceneLoaded += OnSceneLoad;
        //场景卸载时的事件
        //Unity事件一个参数无返回值
        SceneManager.sceneUnloaded += OnSceneUnLoad;
    }
    /// <summary>
    /// 场景卸载
    /// </summary>
    /// <param name="arg0">场景信息</param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnSceneUnLoad(Scene arg0)
	{
		DOTween.KillAll();//在场景卸载之前，杀死所有动画。
        
        
		// Debug.Log("场景中所有对象已经被销毁，即将卸载场景......");
		//Debug.Log("<color=#00ff00>代码来到这里说明场景中所有的对象都被卸载，这里你需要做的是保存一些数据，最后想做的事放到这里来</color>");
	}
	/// <summary>
	/// 这个方法里做一些场景跳转后初始化的逻辑----添加到Start方法终的事件中
	/// </summary>
	/// <param name="arg0">场景信息</param>
	/// <param name="arg1">加载场景模式信息</param>
	private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
	{
		UIManager.Instance.Clear();
		switch (arg0.name)
		{
			case "Unititled":
				// 初始化UIManager栈区和字典
					UIManager.Instance.PushUI("UIStart");
					MusicMgr.Instance.PlayBgm("login");
				// Debug.Log("场景跳转初始化");
				break;
			case "SampleScene":
				// 初始化UIManager栈区和字典
				MusicMgr.Instance.PlayBgm("sairai");
				// Debug.Log("场景跳转初始化");
				break;
		}
	}
    /// <summary>
    /// 在销毁时 解绑事件
    /// </summary>
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
    private void OnDestroy()
#pragma warning restore CS0108 // 成员隐藏继承的成员；缺少关键字 new
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnLoad;
        //Debug.Log("解绑完毕");
    }
    /// <summary>
	/// 加载场景
	/// </summary>
	/// <param name="buildIndex"></param>
	public void LoadScene()
	{
		PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
	}
    public void LoadScene(int buildIndex)
    {
        PhotonNetwork.LoadLevel(buildIndex);
    }
	/// <summary>
	/// 当离开房间，加载第一个场景。
	/// </summary>
    public override void OnLeftRoom()
    {
        if (MainSceneRoot.Instance != null)//如果场景中有这个单例存在，那么执行他清除数据的方法。
        {
            MainSceneRoot.Instance.PlayerDataClear();
            MainSceneRoot.Instance.SceneDataClean();
        }
        LoadScene(0);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PhotonNetwork.AutomaticallySyncScene = false;//取消同步主机场景层级。
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("离开房间");
            PhotonNetwork.LeaveRoom();
        }
    }
}
