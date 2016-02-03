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

    public static bool IsPlaying(string name)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].tweenName == name)
            {
                return tweenList[i].IsPlaying;
            }
        }

        return false;
    }

    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static uTweenBase Play(string name)
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
    public static void Plays(string name)
    {
        for (int i = 0; i < tweenList.Count; i++)
        {
            if (tweenList[i].tweenName == name)
            {
                tweenList[i].Play();
            }
        }
    }

    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static uTweenBase[] GetPlayList(string name)
    {
        return tweenList.FindAll(i => i.tweenName == name).ToArray();
    }
    

    /// <summary>
    /// 再生する。
    /// </summary>
    /// <param name="name"></param>
    public static void Pause(string name)
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
    public static void Resume(string name)
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
    public static void Stop(string name)
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
