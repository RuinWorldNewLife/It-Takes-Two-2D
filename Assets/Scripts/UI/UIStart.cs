using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;//添加引用的
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
/// <summary>
/// 脚本功能：继承于UIBase功能开启设置页面和进入游戏
/// </summary>
public class UIStart : UIBase
{
    public TMP_Text connectingInfo;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//连接到网络
        PhotonNetwork.AutomaticallySyncScene = true;//同步主机场景层级。
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        connectingInfo.text = "Connected To Server";
        connectingInfo.color = Color.green;
    }
    public void GoToRoomListBtn(string uiName)
    {
        PhotonNetwork.JoinLobby();
        UIManager.Instance.PushUI(uiName);
    }
    public void GoToOption(string uiName) 
    {
        UIManager.Instance.PushUI(uiName);
    }
}
