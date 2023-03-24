using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoom : UIBase
{
    #region 变量
    public TMP_Text inRoomInfo;
    public TMP_InputField roomListInfo;
    #endregion
    
    #region PhotonNetwork 回调方法
    /// <summary>
    /// 进入大厅后执行
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        inRoomInfo.text = "InLobbyed";
        inRoomInfo.color = Color.green;
    }
    /// <summary>
    /// 成功创建并且进入房间：
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        inRoomInfo.text = "Success Create Room";
        inRoomInfo.color = Color.green;
        roomListInfo.text = "You are in room.";
    }
    /// <summary>
    /// 房间列表更新执行
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
    /// 进入房间后调用
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


    #region 按钮回调方法
    /// <summary>
    /// 按钮回调返回start
    /// </summary>
    public void OnBackToStartBtn()
    {
        PhotonNetwork.LeaveLobby();
        inRoomInfo.text = "DisInLobby";
        inRoomInfo.color = Color.black;
        UIManager.Instance.PopUI();
    }
    /// <summary>
    /// 创建房间按钮回调
    /// </summary>
    public void OnCreateRoomBtn()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse("2");//设置最大人数为2
        PhotonNetwork.CreateRoom("Default Room", options);
    }
    /// <summary>
    /// 加入房间按钮回调
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
