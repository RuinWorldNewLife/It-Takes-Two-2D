using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinGame : UIBase
{
    /// <summary>
    /// ���ر���
    /// </summary>
    public void GoToReturnTitle()
    {
        if (PhotonNetwork.InRoom)
        {
            //Debug.Log("�뿪����");
            PhotonNetwork.LeaveRoom();
        }
    }
}
