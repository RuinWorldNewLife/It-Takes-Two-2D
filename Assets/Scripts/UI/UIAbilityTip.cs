using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbilityTip : UIBase
{
    
    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void GoToReturnGame()
    {
        UIManager.Instance.PopUI();
        //Time.timeScale = 1;
    }
}
