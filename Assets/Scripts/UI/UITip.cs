using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITip : UIBase
{
    /// <summary>
    /// ����
    /// </summary>
    public void GoBack()
    {
        UIManager.Instance.PopUI();
    }

}
