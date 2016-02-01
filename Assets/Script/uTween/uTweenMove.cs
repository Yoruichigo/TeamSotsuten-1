using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class uTweenMove : uTweenBase {

    public Vector3 startPosition = Vector3.zero;
    public Vector3 targetPosition = Vector3.zero;
    Vector3 tweenStartPos = Vector3.zero;

    /// <summary>
    /// 再生する。
    /// </summary>
    public override void Play()
    {
        isTweening = true;
        pauseTime = 0;
        playTime = 0;
        cashRectTransform.anchoredPosition3D = startPosition;
        tweenStartPos = cashRectTransform.anchoredPosition3D;
    }


    void Update()
    {
        if (!isTweening) return;

        cashRectTransform.anchoredPosition3D = Vector3.Lerp(tweenStartPos, targetPosition, GetCurve());

        Finish();
    }
}
