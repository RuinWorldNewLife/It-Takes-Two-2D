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
		//重新加载当前活跃的场景
		SceneMgr.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void GoToOption(string uiName)
	{
		UIManager.Instance.PushUI(uiName);
	}
	public void GoToReturnTitle()
	{
		SceneMgr.Instance.LoadScene(1);
	}
	public override IEnumerator UpdateAlphaShow()
	{
		// Debug.Log("进来UpdateAlphaShow");
		do
		{
			//Debug.Log("进来While");
			yield return null;
			canvasGroup.alpha += Time.deltaTime*8;
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
			canvasGroup.alpha -= Time.deltaTime*8;
		} while (canvasGroup.alpha > 0);
	}
}
