﻿//-------------------------------------------------------------
//  シングルトンの基底クラス
//  マネージャーは、これを継承してください。
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    Debugger.LogError(typeof(T) + "is nothing");
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// 派生クラス側で必ず、base.Awake() を呼んでください。
    /// </summary>
    public virtual void Awake()
    {
        // TO DO
        // 追加していく
    }

    /// <summary>
    /// 派生クラス側で必ず、base.Start() を呼んでください。
    /// </summary>
    public virtual void Start() 
    {
        // TO DO
        // 追加していく
	}

    /// <summary>
    /// 派生クラス側で必ず、base.Update() を呼んでください。
    /// </summary>
    public virtual void Update() 
    {
        // TO DO
        // 追加していく
	}
}
