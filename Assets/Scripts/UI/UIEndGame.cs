using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndGame : UIBase
{
    /// <summary>
    /// 返回开始界面
    /// </summary>
    public void GoToRestart()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            MainSceneRoot.Instance.Restart();//重新开始游戏
        }
        else
        {
            UIManager.Instance.PushUI("UITip");
        }
    }
    /// <summary>
    /// 返回标题
    /// </summary>
    public void GoToReturnTitle()
    {
        PhotonNetwork.AutomaticallySyncScene = false;//取消同步主机场景层级。
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("离开房间");
            PhotonNetwork.LeaveRoom();
        }
        if (PhotonNetwork.InLobby)
        {
            Debug.Log("离开大厅");
            PhotonNetwork.LeaveLobby();
        }
    }
}
