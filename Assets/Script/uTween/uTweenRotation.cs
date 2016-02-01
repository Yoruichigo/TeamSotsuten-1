using UnityEngine;
using System.Collections;

public class uTweenRotation : uTweenBase
{
    public Vector3 startRotation = Vector3.zero;
    public Vector3 targetRotation = Vector3.zero;
    Vector3 tweenStartRotation = Vector3.zero;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        cashRectTransform.localRotation = Quaternion.Euler(startRotation);
        tweenStartRotation = cashRectTransform.localRotation.eulerAngles;
    }

    void Update()
    {
        if (!isTweening) return;

        cashRectTransform.localRotation = Quaternion.Euler(Vector3.Lerp(tweenStartRotation, targetRotation, GetCurve()));

        Finish();
    }
}
