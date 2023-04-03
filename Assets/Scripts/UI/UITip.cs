using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITip : UIBase
{
    /// <summary>
    /// ·µ»Ø
    /// </summary>
    public void GoBack()
    {
        UIManager.Instance.PopUI();
    }

}
