using UnityEngine;
using System.Collections;

public class uTweenScale : uTweenBase 
{
    public Vector3 startScale = Vector3.zero;
    public Vector3 targetScale = Vector3.zero;
    Vector3 tweenStartScale = Vector3.zero;

    /// <summary>
    /// 再生する。
    /// </summary>
    public override void Play()
    {
        isTweening = true;
        pauseTime = 0;
        playTime = 0;

        cashRectTransform.localScale = startScale;
        tweenStartScale = cashRectTransform.localScale;
    }

    void Update()
    {
        if (!isTweening) return;

        cashRectTransform.localScale = Vector3.Lerp(tweenStartScale, targetScale, GetCurve());
        
        Finish();

    }
}
