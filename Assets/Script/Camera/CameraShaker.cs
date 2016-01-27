using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// http://kidooom.hatenadiary.jp/entry/20140330/1396194258
/// より。
/// </summary>

public class CameraShaker : MonoBehaviour
{
    /// <summary>
    /// 振動の減少量
    /// </summary>
    [SerializeField]
    private float shakeDecay = 3f;
    
    /// <summary>
    /// 振動開始時の振動の大きさ
    /// </summary>
    [SerializeField]
    private float coefShakeIntensity = 50f;

    /// <summary>
    /// 初期位置
    /// </summary>
    private Vector3 originPosition = Vector3.zero;

    /// <summary>
    /// 現在の振動の大きさ
    /// </summary>
    static private float shakeIntensity = 0f;


    static private Vector3 randomRate = Vector3.zero;


    void Start()
    {
        Initialize();
    }

    void Initialize()
    {

        ///座標の初期値保存
        originPosition = transform.localPosition;

    }

    void Update()
    {
        if (shakeIntensity > 0)
        {
            randomRate = Random.insideUnitSphere;

            transform.localPosition = originPosition + randomRate * shakeIntensity;

            shakeIntensity -= shakeDecay;
        }
        else
        {
            transform.localPosition = originPosition;
            shakeIntensity = 0f;

        }
    }

    /// <summary>
    /// 振動させる
    /// </summary>
    public void Shake()
    {
        originPosition = transform.localPosition;
        shakeIntensity = coefShakeIntensity;
    }

}
