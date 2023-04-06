using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINoTip : UIBase
{
    public void GoToReturnGame()
    {
        UIManager.Instance.PopUI();
        //Time.timeScale = 1;
    }
}
