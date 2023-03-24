using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton<T> : MonoBehaviourPunCallbacks where T : class
{
    public static T _baseSingleTon;
    
    public static T Instance
    {
        get
        {
            if(_baseSingleTon == null)
            {
                _baseSingleTon = (T)Activator.CreateInstance(typeof(T),true);
            }
            return _baseSingleTon;
        }
    }
}
