using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UINextWaveRenderer : MonoBehaviour 
{
    enum State
    { 
        NULL,
        OPEN,       //< 再生開始
        UPDATE,     //< アップデート処理    
        CLOSE,      //< 再生終了処理
    }

    State state = State.NULL;
    uTweenBase[] flashPlayList = null;

    void Update()
    {
        switch (state)
        {
            case State.OPEN:
                flashPlayList = uTween.GetPlayList("NextWaveFlash");
                for (int i = 0; i < flashPlayList.Length; i++)
                {
                    flashPlayList[i].cashRectTransform.localScale = Vector3.one;
                    flashPlayList[i].Play();
                }

                state = State.UPDATE;
                break;

            case State.UPDATE:

                if (!flashPlayList[0].IsPlaying)
                {
                    state = State.CLOSE;
                }
                break;

            case State.CLOSE:
                
                var playList = uTween.GetPlayList("NextWave");
                for (int i = 0; i < playList.Length; i++)
                {
                    if (playList[i].IsPlaying) continue;

                    playList[i].Play();
                }

                state = State.NULL;
                break;
        }
    }

    /// <summary>
    /// アニメーションを再生する。
    /// </summary>
    public void AnimationStart()
    {
        state = State.OPEN;
    }
}
