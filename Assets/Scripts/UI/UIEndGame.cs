using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndGame : UIBase
{
    /// <summary>
    /// ���ؿ�ʼ����
    /// </summary>
    public void GoToRestart()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            MainSceneRoot.Instance.Restart();//���¿�ʼ��Ϸ
        }
        else
        {
            UIManager.Instance.PushUI("UITip");
        }
    }
    /// <summary>
    /// ���ر���
    /// </summary>
    public void GoToReturnTitle()
    {
        PhotonNetwork.AutomaticallySyncScene = false;//ȡ��ͬ�����������㼶��
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("�뿪����");
            PhotonNetwork.LeaveRoom();
        }
        if (PhotonNetwork.InLobby)
        {
            Debug.Log("�뿪����");
            PhotonNetwork.LeaveLobby();
        }
    }
}
