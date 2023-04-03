using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainSceneRoot : Singleton<MainSceneRoot>
{
    GameObject player;
    bool isInited;//判断是否进入
    PhotonView windPhotonView;

    [HideInInspector]
    public Vector3 RedInitPos;
    [HideInInspector]
    public Vector3 DarkInitPos;
    [Header("场景数据")]
    [SerializeField]
    private SceneData sceneData;//场景数据

    //玩家生成位置的存储结构体
    public struct CharacterStartPos
    {
        public Vector3 RedInitPos;
        public Vector3 DarkInitPos;
    }

    private void Awake()
    {
        isInited = false;
        windPhotonView = GetComponent<PhotonView>();
    }

    
    private void Start()
    {
        SetBornPos();//初始化
        //设置成功加载玩家属性
        SetPlayerLoaded();

        //定时生成风的方法
        MonoHelper.Instance.InvokeReapeat(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("winkHorizontal", new Vector3(67.28f, 1.97f, 0f), Quaternion.identity);
            }
        }, 6, () => { return false; });
        
    }

    /// <summary>
    /// 设置出生位置
    /// </summary>
    public void SetBornPos()
    {
        //第一个生成位置
        CharacterStartPos Pos1 = new CharacterStartPos();
        Pos1.RedInitPos = new Vector3(-0.64f, 2.4f, 0f);
        Pos1.DarkInitPos = new Vector3(-3f, 2.4f, 0f);

        //第二个生成位置
        CharacterStartPos Pos2 = new CharacterStartPos();
        Pos2.RedInitPos = new Vector3(28f, 5f, 0f);
        Pos2.DarkInitPos = new Vector3(26.81f, 5f, 0f);

        //第三个生成位置
        CharacterStartPos Pos3 = new CharacterStartPos();
        Pos3.RedInitPos = new Vector3(70f, 7.2f, 0f);
        Pos3.DarkInitPos = new Vector3(68f, 7.2f, 0f);

        switch (sceneData.bornPosIndex)
        {
            case 1:
                RedInitPos = Pos1.RedInitPos;
                DarkInitPos = Pos1.DarkInitPos;
                break;
            case 2:
                RedInitPos = Pos2.RedInitPos;
                DarkInitPos = Pos2.DarkInitPos;
                break;
            case 3:
                RedInitPos = Pos3.RedInitPos;
                DarkInitPos = Pos3.DarkInitPos;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 设置当前玩家已经加载场景成功。
    /// </summary>
    private void SetPlayerLoaded()
    {
        Hashtable hash = new Hashtable();
        //添加属性
        hash.Add("Loaded", true);
        //设置属性
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /// <summary>
    /// 每次玩家的属性发生改变（添加了信息）调用此方法。
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        //检测所有玩家是否加载场景成功
        CheckAllPlayerLoaded();
    }

    /// <summary>
    /// 检测所有玩家是否都已经加载成功。
    /// </summary>
    /// <returns></returns>
    private bool CheckAllPlayerLoaded()
    {
        //如果是不是主客户端，则直接返回否。
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //是否加载
            object isLoaded = false;
            //尝试加载属性
            PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("Loaded", out isLoaded);

            //如果没有加载到
            if (isLoaded == null || !(bool)isLoaded)
            {
                return false;
            }
        }
        //设置房间属性
        Hashtable initHero = new Hashtable();
        initHero.Add("InitHero", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(initHero);
        //表示所有玩家都已经加载场景成功。
        return true;
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object canInit = false;
        //尝试获取属性
        propertiesThatChanged.TryGetValue("InitHero", out canInit);
        //属性获取到，且值为true
        try
        {
            if ((bool)canInit && !isInited)
            {
                //所有玩家生成各自的英雄。
                InitHero();
            }
        }
        catch (System.Exception)
        {
        }

    }

    //将角色加载进入场景
    private void InitHero()
    {

        //如果角色进入场景，则将判断设置为true
        isInited = true;
        if (PhotonNetwork.IsMasterClient)
        {
            //生成网络对象
            player = PhotonNetwork.Instantiate("PlayerRed", RedInitPos, Quaternion.identity);
        }
        else
        {
            //生成网络对象
            player = PhotonNetwork.Instantiate("PlayerDark", DarkInitPos, Quaternion.identity);
        }
        //将玩家的名字中的克隆取出
        player.name = player.name.Replace("(Clone)", "");
    }
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void Restart()
    {
        player.GetComponent<Player>().Restart();//重新在存储位置开始游戏。
    }
    /// <summary>
    /// 清除玩家数据，重置生成位置，在离开房间时调用。
    /// </summary>
    public void PlayerDataClear()
    {
        try
        {
            sceneData.bornPosIndex = 1;//重置生成位置。
            player.GetComponent<Player>().playerDataClear();//清除玩家数据。
        }
        catch (System.Exception)
        {
        }
        
    }
}
