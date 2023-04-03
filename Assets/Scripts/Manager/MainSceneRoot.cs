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

    [HideInInspector]
    public Vector3 RedInitPos;
    [HideInInspector]
    public Vector3 DarkInitPos;
    [Header("��������")]
    [SerializeField]
    private SceneData sceneData;//��������

    //�������λ�õĴ洢�ṹ��
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
        SetBornPos();//��ʼ��
        //���óɹ������������
        SetPlayerLoaded();

        //��ʱ���ɷ�ķ���
        MonoHelper.Instance.InvokeReapeat(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("winkHorizontal", new Vector3(67.28f, 1.97f, 0f), Quaternion.identity);
            }
        }, 6, () => { return false; });
        
    }

    /// <summary>
    /// ���ó���λ��
    /// </summary>
    public void SetBornPos()
    {
        //��һ������λ��
        CharacterStartPos Pos1 = new CharacterStartPos();
        Pos1.RedInitPos = new Vector3(-0.64f, 2.4f, 0f);
        Pos1.DarkInitPos = new Vector3(-3f, 2.4f, 0f);

        //�ڶ�������λ��
        CharacterStartPos Pos2 = new CharacterStartPos();
        Pos2.RedInitPos = new Vector3(28f, 5f, 0f);
        Pos2.DarkInitPos = new Vector3(26.81f, 5f, 0f);

        //����������λ��
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
    /// <summary>
    /// ���¿�ʼ��Ϸ
    /// </summary>
    public void Restart()
    {
        player.GetComponent<Player>().Restart();//�����ڴ洢λ�ÿ�ʼ��Ϸ��
    }
    /// <summary>
    /// ���������ݣ���������λ�ã����뿪����ʱ���á�
    /// </summary>
    public void PlayerDataClear()
    {
        try
        {
            sceneData.bornPosIndex = 1;//��������λ�á�
            player.GetComponent<Player>().playerDataClear();//���������ݡ�
        }
        catch (System.Exception)
        {
        }
        
    }
}
