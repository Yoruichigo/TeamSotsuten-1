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
    private float shakeDecay = 0.001f;
    
    /// <summary>
    /// 振動開始時の振動の大きさ
    /// </summary>
    [SerializeField]
    private float coefShakeIntensity = 0.01f;

    /// <summary>
    /// カメラの初期位置
    /// </summary>
    private Vector3 originPosition = Vector3.zero;

//    private Vector3 originScale = Vector3.one;

    /// <summary>
    /// 現在の振動の大きさ
    /// </summary>
    private float shakeIntensity = 0f;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {

        ///座標の初期値保存
        originPosition = transform.localPosition;

        ///回転の初期値保存
//        originScale = transform.localScale;

    }

    void Update()
    {
        

        if (shakeIntensity > 0)
        {
            transform.localPosition = originPosition + Random.insideUnitSphere * shakeIntensity;
            //childrenCameras[index].rotation = new Quaternion(
            //                                 originRotation[index].x + Random.Range(-shakeIntensity, shakeIntensity) * 2f,
            //                                 originRotation[index].y + Random.Range(-shakeIntensity, shakeIntensity) * 2f,
            //                                 originRotation[index].z + Random.Range(-shakeIntensity, shakeIntensity) * 2f,
            //                                 originRotation[index].w + Random.Range(-shakeIntensity, shakeIntensity) * 2f);
            shakeIntensity -= shakeDecay;
        }
        else
        {
//                childrenCameras[index].rotation = originRotation[index];
            //transform.localPosition = originPosition;
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
