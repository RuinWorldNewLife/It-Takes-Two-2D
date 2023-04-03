using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 脚本功能：暂停游戏
/// </summary>
public class UIGamePause : UIBase
{
    //界面的的子物体
    public void GoToReturnGame()
    {
        UIManager.Instance.PopUI();
        //Time.timeScale = 1;
    }
    public void GoToContinueGame()
    {
        UIManager.Instance.PopUI();
        //Time.timeScale = 1;
    }
    public void GoToRestart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UIManager.Instance.PopUI();//将窗口取消
            MainSceneRoot.Instance.Restart();//重新开始游戏
        }
        else
        {
            UIManager.Instance.PushUI("UITip");
        }
    }

    /// <summary>
    /// 打开设置UI
    /// </summary>
    /// <param name="uiName"></param>
    public void GoToOption(string uiName)
    {
        UIManager.Instance.PushUI(uiName);
    }
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
    public override IEnumerator UpdateAlphaShow()
    {
        // Debug.Log("进来UpdateAlphaShow");
        do
        {
            //Debug.Log("进来While");
            yield return null;
            canvasGroup.alpha += Time.deltaTime * 8;
        } while (canvasGroup.alpha < 1);
        canvasGroup.interactable = true; //交互开始
        canvasGroup.blocksRaycasts = true; //检测射线
        graphicRaycaster.enabled = true; //检测开始
    }

    public override IEnumerator UpdateAlphaHide()
    {
        canvasGroup.interactable = false; //不去交互
        canvasGroup.blocksRaycasts = false; //不去检测射线
        graphicRaycaster.enabled = false; //检测关闭
        do
        {
            yield return null;
            canvasGroup.alpha -= Time.deltaTime * 8;
        } while (canvasGroup.alpha > 0);
    }

}
