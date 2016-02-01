using UnityEngine;
using System.Collections;

public class uTweenBase : MonoBehaviour
{
    public enum LoopType
    { 
        Once,
        Loop,
        PingPong,
    }

    public string tweenName = "";
    public float tweenTime = 0.0f;
    public LoopType loopType = LoopType.Once;
    
    public AnimationCurve tweenCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool isAwakePlay = false;

    public RectTransform cashRectTransform = null;
    protected float pauseTime = 0;

    protected bool isTweening = false;
    protected bool isBackMove = false;
    protected float playTime = 0;

    public void Awake()
    {
        uTween.Register(this);

        cashRectTransform = transform as RectTransform;

        if (cashRectTransform == null)
        {
            Debug.LogError("RectTransformがありません。");
            return;
        }

        if (isAwakePlay)
        {
            Play();
        }
    }

    public virtual void Play() { }

    /// <summary>
    /// 一時停止する。
    /// </summary>
    public void Pause()
    {
        isTweening = false;
        pauseTime = playTime;
    }

    /// <summary>
    /// 一時停止を再生する。
    /// </summary>
    public void Resume()
    {
        isTweening = true;
        playTime = pauseTime;
    }

    /// <summary>
    /// 一時停止を再生する。
    /// </summary>
    public void Stop()
    {
        playTime = 0;
        pauseTime = 0;
        isTweening = false;
    }

    protected float GetCurve()
    {
        if (isBackMove)
        {
            playTime -= Time.deltaTime;
        }
        else
        {
            playTime += Time.deltaTime;
        }

        float rate = playTime / tweenTime;

        return tweenCurve.Evaluate(rate);

    }

    protected void Finish()
    {

        if (playTime < 0.0 && loopType == LoopType.PingPong)
        {
            isBackMove = false;
        }

        if (playTime >= tweenTime)
        {
            switch (loopType)
            {
                case LoopType.Loop:
                    Play();
                    break;

                case LoopType.PingPong:
                    isBackMove = true;
                    break;

                case LoopType.Once:
                    isTweening = false;
                    break;
            }
        }
    }
}