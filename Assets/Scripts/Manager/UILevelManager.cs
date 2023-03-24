using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelManager : BaseSingleton<UILevelManager>
{
    private void Start()
    {
        UIManager.Instance.PushUI("UIStart");
        MusicMgr.Instance.PlayBgm("login");
        
    }
}
