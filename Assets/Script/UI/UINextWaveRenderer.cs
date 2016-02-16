using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UINextWaveRenderer : MonoBehaviour 
{

    /// <summary>
    /// 有効にするか設定。
    /// </summary>
    public void AnimationStart()
    {
        var playList = uTween.GetPlayList("NextWave");
        for (int i = 0; i < playList.Length; i++)
        {
            playList[i].Play();
        }
    }
}
