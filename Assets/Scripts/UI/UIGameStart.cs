using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建人：朱泽辉
/// 脚本功能：游戏开始游戏界面
/// </summary>
public class UIGameStart : UIBase
{
	public void DoOnGamePause(string uiName)
	{
		UIManager.Instance.PushUI(uiName);
		//Time.timeScale = 0;
	}
}
