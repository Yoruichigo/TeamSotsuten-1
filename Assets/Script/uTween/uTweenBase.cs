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

    public string tweenName = "";       //< 処理するための名前
    public float tweenTime = 0.0f;      //< 時間
    public LoopType loopType = LoopType.Once;   //< ループ種類
    
    // アニメーションカーブ
    public AnimationCurve tweenCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public bool isAwakePlay = false;    //< 最初で再生するかどうか

    public RectTransform cashRectTransform = null;  //< キャッシュしておくRectTransform

    [SerializeField,Range(0.0f,1.0f)]
    float rate = 0;
    
    protected float pauseTime = 0;      //< ポーズ時間

    protected bool isTweening = false;  //< Tween中かどうか
    protected bool isPlayBack = false;  //< 逆再生かどうか
    protected float playTime = 0;       //< 再生時間

    public bool IsPlaying { get { return isTweening; } }

    public void Awake()
    {
        cashRectTransform = transform as RectTransform;

        if (cashRectTransform == null)
        {
            Debug.LogError("RectTransformがありません。");
            return;
        }

        uTween.Register(this);

        if (isAwakePlay)
        {
            Play();
        }
    }

    /// <summary>
    /// 各種初期化
    /// </summary>
    public virtual void Init() { }
    
    /// <summary>
    /// 再生
    /// </summary>
    public void Play()
    {
        isTweening = true;
        isPlayBack = false;
        pauseTime = 0;
        playTime = 0;

        Init();

    }

    
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
        isPlayBack = false;
        playTime = 0;
        pauseTime = 0;
        isTweening = false;
    }

    protected float GetCurve()
    {
        playTime += isPlayBack ? -Time.deltaTime : Time.deltaTime;

        rate = playTime / tweenTime;

        return tweenCurve.Evaluate(rate);
    }

    protected void Finish()
    {
        if (playTime < 0.0 && loopType == LoopType.PingPong)
        {
            isPlayBack = false;
        }

        if (playTime >= tweenTime)
        {
            switch (loopType)
            {
                case LoopType.Loop:
                    Play();
                    break;

                case LoopType.PingPong:
                    isPlayBack = true;
                    break;

                case LoopType.Once:
                    isTweening = false;
                    break;
            }
        }
    }
}