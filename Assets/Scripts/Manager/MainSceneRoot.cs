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
    bool isInited;//�ж��Ƿ����
    PhotonView windPhotonView;
    public Vector3 RedInitPos;
    public Vector3 DarkInitPos;

    //Camera mainCamera;
    private void Awake()
    {
        isInited = false;
        windPhotonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        //���óɹ������������
        SetPlayerLoaded();

        ////��ʱ���ɷ�ķ���
        //MonoHelper.Instance.InvokeReapeat(() =>
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        PhotonNetwork.Instantiate("wink", new Vector3(-4.63f, 11.82f, 0f), Quaternion.identity);
        //    }
        //}, 6, () => { return false; });
    }
    
    /// <summary>
    /// ���õ�ǰ����Ѿ����س����ɹ���
    /// </summary>
    private void SetPlayerLoaded()
    {
        Hashtable hash = new Hashtable();
        //�������
        hash.Add("Loaded", true);
        //��������
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    /// <summary>
    /// ÿ����ҵ����Է����ı䣨�������Ϣ�����ô˷�����
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        //�����������Ƿ���س����ɹ�
        CheckAllPlayerLoaded();
    }

    /// <summary>
    /// �����������Ƿ��Ѿ����سɹ���
    /// </summary>
    /// <returns></returns>
    private bool CheckAllPlayerLoaded()
    {
        //����ǲ������ͻ��ˣ���ֱ�ӷ��ط�
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //�Ƿ����
            object isLoaded = false;
            //���Լ�������
            PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("Loaded", out isLoaded);

            //���û�м��ص�
            if (isLoaded == null || !(bool)isLoaded)
            {
                return false;
            }
        }
        //���÷�������
        Hashtable initHero = new Hashtable();
        initHero.Add("InitHero", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(initHero);
        //��ʾ������Ҷ��Ѿ����س����ɹ���
        return true;
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        object canInit = false;
        //���Ի�ȡ����
        propertiesThatChanged.TryGetValue("InitHero", out canInit);
        //���Ի�ȡ������ֵΪtrue
        try
        {
            if ((bool)canInit && !isInited)
            {
                //����������ɸ��Ե�Ӣ�ۡ�
                InitHero();
            }
        }
        catch (System.Exception)
        {
        }
        
    }
    
    //����ɫ���ؽ��볡��
    private void InitHero()
    {

        //�����ɫ���볡�������ж�����Ϊtrue
        isInited = true;
        if (PhotonNetwork.IsMasterClient)
        {
            //�����������
            player = PhotonNetwork.Instantiate("PlayerRed", RedInitPos, Quaternion.identity);
        }
        else
        {
            //�����������
            player = PhotonNetwork.Instantiate("PlayerDark", DarkInitPos, Quaternion.identity);
        }
        //����ҵ������еĿ�¡ȡ��
        player.name = player.name.Replace("(Clone)", "");

        
    }
}
