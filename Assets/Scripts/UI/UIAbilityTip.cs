using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbilityTip : UIBase
{
    
    /// <summary>
    /// ∑µªÿ”Œœ∑
    /// </summary>
    public void GoToReturnGame()
    {
        UIManager.Instance.PopUI();
        //Time.timeScale = 1;
    }
}
