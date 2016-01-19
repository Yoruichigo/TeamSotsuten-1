//-------------------------------------------------------------
//  ウォッチ管理クラス
//  加速度センサー・ジャイロ
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WatchManager : Singleton<WatchManager>
{
    List<Text> debugTextList = new List<Text>();

    PhotonView view = null;

    /// <summary>
    /// 加速度センサー
    /// </summary>
    public Vector3 Acc { get; private set; }

    /// <summary>
    /// ジャイロセンサー
    /// </summary>
    public Vector3 GyroAngle { get; private set; }
    
    public override void Awake()
    {
        base.Awake();

        view = GetComponent<PhotonView>();

        Input.gyro.enabled = true;

        var child = transform.GetComponentsInChildren<Canvas>();

        for (int i = 0; i < child.Length; i++)
        {
            debugTextList.Add(child[i].GetComponentInChildren<Text>());
        }

    }

    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();

        if (ConnectionManager.IsWacth)
        {
            var acc = Input.acceleration;
            var gyroAngle = Input.gyro.attitude.eulerAngles;

            view.RPC("SyncData", 
                ConnectionManager.GetSmartPhonePlayer(), 
                new object[] { acc, gyroAngle });
            
#if UNITY_EDITOR
            DebugTextShow(acc, gyroAngle);
#endif
        }
    }

    [PunRPC]
    void SyncData(Vector3 acc,Vector3 gyroAngle,PhotonMessageInfo info)
    {
        Acc = acc;
        GyroAngle = gyroAngle;

#if UNITY_EDITOR
        DebugTextShow(Acc, GyroAngle);
#endif
    }

    void DebugTextShow(Vector3 acc, Vector3 gyroAngle)
    {
        for (int i = 0; i < debugTextList.Count; i++)
        {
            debugTextList[i].text = "Acc : " + acc.ToString() + "\n";
            debugTextList[i].text += "GyroAngle : " + gyroAngle.ToString();
        }
    }

    /// <summary>
    /// 書かないといけない関数
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
