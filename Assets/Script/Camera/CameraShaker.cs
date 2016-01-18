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

    enum CameraID
    {
        LEFT = 0,
        RIGHT
    }

    const int cameraCount = 2;

    /// <summary>
    /// 左右のカメラ
    /// </summary>
    Transform[] childrenCameras = new Transform[cameraCount];
    
    /// <summary>
    /// 振動開始時の振動の大きさ
    /// </summary>
    [SerializeField]
    private float coefShakeIntensity = 0.01f;

    /// <summary>
    /// カメラの初期位置
    /// </summary>
    private Vector3[] originPosition = new Vector3[cameraCount];

    /// <summary>
    /// カメラの初期傾き
    /// </summary>
    private Quaternion[] originRotation = new Quaternion[cameraCount];

    /// <summary>
    /// 現在の振動の大きさ
    /// </summary>
    private float shakeIntensity;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        RegisterCamera();

        ///座標の初期値保存
        originPosition[(int)CameraID.LEFT] = childrenCameras[(int)CameraID.LEFT].position;
        originPosition[(int)CameraID.RIGHT] = childrenCameras[(int)CameraID.RIGHT].position;

        ///回転の初期値保存
        originRotation[(int)CameraID.LEFT] = childrenCameras[(int)CameraID.LEFT].rotation;
        originRotation[(int)CameraID.RIGHT] = childrenCameras[(int)CameraID.RIGHT].rotation;

    }

    void RegisterCamera()
    {
        for (int index = 0; index < cameraCount; ++index) 
        {
            childrenCameras[index] = transform.GetChild(cameraCount);
        }

        foreach (var camera in childrenCameras)
        {
            if (camera == null)
            {
                Debug.LogError("左右のどちらかのカメラが登録されていません");
            }
        }
    }



    void Update()
    {
        for (int index = 0; index < cameraCount; ++index)
        {
            if (shakeIntensity > 0)
            {
                childrenCameras[index].position = originPosition[index] + Random.insideUnitSphere * shakeIntensity;
                childrenCameras[index].rotation = new Quaternion(
                                                 originRotation[index].x + Random.Range(-shakeIntensity, shakeIntensity) * 2f,
                                                 originRotation[index].y + Random.Range(-shakeIntensity, shakeIntensity) * 2f,
                                                 originRotation[index].z + Random.Range(-shakeIntensity, shakeIntensity) * 2f,
                                                 originRotation[index].w + Random.Range(-shakeIntensity, shakeIntensity) * 2f);
                shakeIntensity -= shakeDecay;
            }
            else
            {
                childrenCameras[index].rotation = originRotation[index];
                childrenCameras[index].position = originPosition[index];

            }
        }
    }

    /// <summary>
    /// 振動させる
    /// </summary>
    public void Shake()
    {
        originPosition[(int)CameraID.LEFT] = childrenCameras[(int)CameraID.LEFT].position;
        originPosition[(int)CameraID.RIGHT] = childrenCameras[(int)CameraID.RIGHT].position;

        shakeIntensity = coefShakeIntensity;
    }

}
