using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 脚本功能：继承于UIBase功能设置音乐和返回后上个界面
/// </summary>
public class UIOption : UIBase
{

    private void Start()
    {
        
    }
    /// <summary>
    /// 返回
    /// </summary>
    public void GoBack() 
    {
        UIManager.Instance.PopUI();
    }

    
    /// <summary>
    /// 背景音乐是否静音(BGM)
    /// </summary>
    public void MusicToggleValueChange(bool isOn)
    {
        MusicMgr.Instance.Mute = !isOn;
    }
    public void MusicVolumeValueChange(Slider sliderValue)
    {
        MusicMgr.Instance.Volume = sliderValue.value;
    }
    public void SoundsToggleValueChange(bool isOn)
    {
        MusicMgr.Instance.SoundsMute = !isOn;
    }
    public void SoundsVolumeValueChange(float sliderValue)
    {
        MusicMgr.Instance.SoundsVolume = sliderValue;
    }
}
