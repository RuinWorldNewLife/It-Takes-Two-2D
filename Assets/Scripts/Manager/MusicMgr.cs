using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 脚本功能 管理音乐播放
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicMgr : Singleton<MusicMgr>
{
	private AudioSource audioBGM;
	private AudioSource audioSounds;
	private string audioDir = "Audio";
	private Dictionary<string, AudioClip> audioDic;
	//背景音乐是否静音
	//音效是否静音


	public bool Mute
	{
		set
		{
			audioBGM.mute = value;
		}
		get
		{
			return audioBGM.mute;
		}
	}
	//背景音乐调整音量
	public float Volume
	{
		set
		{
			audioBGM.volume = value;
		}
		get
		{
			return audioBGM.volume;
		}
	}
	public bool SoundsMute
	{
		set
		{
			audioSounds.mute = value;
		}
		get
		{
			return audioSounds.mute;
		}
	}
	//音乐调整音量
	public float SoundsVolume
	{
		set
		{
			audioSounds.volume = value;
		}
		get
		{
			return audioSounds.volume;
		}
	}
	private void Awake()
	{
		audioSounds = transform.Find("SoundsAudio").GetComponent<AudioSource>();
		audioBGM = GetComponent<AudioSource>();
		audioDic = new Dictionary<string, AudioClip>();
		audioBGM.volume = 1;
		audioBGM.playOnAwake = false;//是否自动播放
		audioBGM.loop = true;//开启循环
		//播放音效
		//AudioSource.PlayClipAtPoint();
	}

	public void PlayBgm(string audioName)
	{
		AudioClip audioClip = GetBgm(audioName);
		audioBGM.clip = audioClip;//指定音乐文件
		audioBGM.Play();//开始播放
		//根据音频 播放音乐
	}
	
	public AudioClip GetBgm(string audioName)
	{
		if (audioDic.ContainsKey(audioName))
		{
			return audioDic[audioName];
		}
		//根据名字找到对应音频
		AudioClip audioClip = Resources.Load<AudioClip>(audioDir+"/"+audioName);
		audioDic.Add(audioName,audioClip);
		return audioClip;
	}
	/// <summary>
	/// 暂停音乐
	/// </summary>
	public void StopBgm()
	{
		//clip制为空
		audioBGM.clip = null;
		audioBGM.Stop();
	}
	
	//播放音效
	public void PlaySounds(string audioname)
	{		
		if (SoundsMute) return;
		//加载音效文件
		AudioClip audioClip = Resources.Load<AudioClip>(audioDir + "/" + audioname);
		//audioClip
		//在指定位置播放音效
		//AudioSource.PlayClipAtPoint(audioClip,position,soundsVolume);
		audioSounds.PlayOneShot(audioClip);
	}
	//

}
