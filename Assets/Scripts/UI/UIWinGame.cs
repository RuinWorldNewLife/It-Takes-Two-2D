using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinGame : UIBase
{
    /// <summary>
    /// 返回标题
    /// </summary>
    public void GoToReturnTitle()
    {
        if (PhotonNetwork.InRoom)
        {
            //Debug.Log("离开房间");
            PhotonNetwork.LeaveRoom();
        }
    }
}
