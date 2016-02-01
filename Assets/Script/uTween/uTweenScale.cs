using UnityEngine;
using System.Collections;

public class uTweenScale : uTweenBase 
{
    public Vector3 startScale = Vector3.zero;
    public Vector3 targetScale = Vector3.zero;
    Vector3 tweenStartScale = Vector3.zero;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
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
