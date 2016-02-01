using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class uTween 
{
    static List<uTweenBase> tweenList = new List<uTweenBase>();

    /// <summary>
    /// 登録する。
    /// </summary>
    /// <param name="handle"></param>
    public static void Register(uTweenBase handle)
    {
        tweenList.Add(handle);
    }

    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static uTweenBase Play(ref string name)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].tweenName == name)
            {
                tweenList[i].Play();
                return tweenList[i];
            }
        }

        return null;
    }
    

    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static void Pause(ref string name)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].tweenName == name)
            {
                tweenList[i].Pause();
                return;
            }
        }
    }


    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static void Resume(ref string name)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].tweenName == name)
            {
                tweenList[i].Resume();
                return;
            }
        }
    }


    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static void Stop(ref string name)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].tweenName == name)
            {
                tweenList[i].Stop();
                return;
            }
        }
    }

}
