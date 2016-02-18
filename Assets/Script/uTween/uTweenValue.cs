using UnityEngine;
using System.Collections;
using System;

public class uTweenValue : uTweenBase {

    public float tweenStartValue = 0;
    public float tweenEndValue = 0;
    float getValue = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        getValue = 0;
    }

    public float GetValue()
    {
        return getValue;
    }

    void Update()
    {
        if (!isTweening) return;

        getValue = Mathf.Lerp(tweenStartValue, tweenEndValue, GetCurve());

        Finish();
    }
}
