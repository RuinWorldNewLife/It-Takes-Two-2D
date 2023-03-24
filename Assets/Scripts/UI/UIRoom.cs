using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoom : UIBase
{
    #region ����
    public TMP_Text inRoomInfo;
    public TMP_InputField roomListInfo;
    #endregion
    
    #region PhotonNetwork �ص�����
    /// <summary>
    /// ���������ִ��
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        inRoomInfo.text = "InLobbyed";
        inRoomInfo.color = Color.green;
    }
    /// <summary>
    /// �ɹ��������ҽ��뷿�䣺
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        inRoomInfo.text = "Success Create Room";
        inRoomInfo.color = Color.green;
        roomListInfo.text = "You are in room.";
    }
    /// <summary>
    /// �����б����ִ��
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        if(roomList.Count > 0 )
        {
            roomListInfo.text = "Room" + roomList[0].Name + "is now exiting";
        }
    }
    /// <summary>
    /// ���뷿������
    /// </summary>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if(newPlayer != PhotonNetwork.MasterClient)
        {
            SceneMgr.Instance.LoadScene();
        }
    }
    #endregion


    #region ��ť�ص�����
    /// <summary>
    /// ��ť�ص�����start
    /// </summary>
    public void OnBackToStartBtn()
    {
        PhotonNetwork.LeaveLobby();
        inRoomInfo.text = "DisInLobby";
        inRoomInfo.color = Color.black;
        UIManager.Instance.PopUI();
    }
    /// <summary>
    /// �������䰴ť�ص�
    /// </summary>
    public void OnCreateRoomBtn()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse("2");//�����������Ϊ2
        PhotonNetwork.CreateRoom("Default Room", options);
    }
    /// <summary>
    /// ���뷿�䰴ť�ص�
    /// </summary>
    public void OnJoinRoomBtn()
    {
        if (!PhotonNetwork.InLobby)
        {
            return;
        }
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion
}
