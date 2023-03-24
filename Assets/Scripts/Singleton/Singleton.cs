using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 创建人：
/// 脚本功能
/// </summary>
public class Singleton<T> : MonoBehaviourPunCallbacks where T : Singleton<T>
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					instance = new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
				}
				else
				{
					instance.Init();//在脚本挂载时调用并且在Awake之后调用
				}
			}

			return instance;
		}
	}
	
	private void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
			Init();//初始化
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	protected virtual void Init()
	{
	}
	//销毁单例
	protected void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
}