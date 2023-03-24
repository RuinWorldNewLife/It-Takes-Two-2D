using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建人：朱泽辉
/// 脚本功能：检测相机是否被销毁
/// </summary>
public class CameraIsDestroy : MonoBehaviour
{
	private Camera _camera;
	private void Start()
	{
		if (Camera.main == null)
		{
			this._camera = GetComponent<Camera>();
			Debug.Log("Camera.main is null");
		}
		Debug.Log(Camera.main);
	}
	// GameManagerScript 
	// private void Start()
	// {
	// Debug.LogError(Camera.main);
	// gameManager = FindObjectOfType<GameManagerScript>();
	// if (Camera.main == null)
	// {
	// 	Debug.LogError("Camera.main is null");
	// }
	// else
	// {
	// 	gameManager.LoadMusic(Camera.main);
	// 	gameManager.LoadAmbient(Camera.main);
	// 	gameManager.FadeStereoPan(Camera.main.gameObject.GetComponent<SwipeActivator>().stereoPanInNode);
	// }
}
