//-------------------------------------------------------------
//  ウォッチの情報を記録するクラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class WatchDeviceInfoRecorder : MonoBehaviour 
{

    [SerializeField]
    Button recordButton = null;

    [SerializeField]
    Text recordText = null;

    /// <summary>
    /// 加速度リスト
    /// </summary>
    List<string> accList = new List<string>();
    
    /// <summary>
    /// ジャイロリスト
    /// </summary>
    List<string> gyroList = new List<string>();

    /// <summary>
    /// 記録をするかどうか
    /// </summary>
    bool isRecording = false;

    /// <summary>
    /// 書き込むIndex
    /// </summary>
    int writeIndex = 0;


	void Start () 
    {
#if UNITY_EDITOR

        recordButton.onClick.AddListener(SwitchRecordState);

        recordButton.image.color = new Color(0, 0, 1);
        recordText.text = "記録開始";
#endif
    }

    /// <summary>
    /// デバッグ停止
    /// 描画をさせない。
    /// </summary>
    public void StopDebugShow()
    {
        recordButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// デバッグ開始
    /// 描画スタートする。
    /// </summary>
    public void StartDebugShow()
    {
#if UNITY_EDITOR
        recordButton.gameObject.SetActive(true);
#endif

    }

    /// <summary>
    /// 記録状態を切り替える
    /// </summary>
    void SwitchRecordState()
    {
#if UNITY_EDITOR

        isRecording = isRecording ? false : true;

        if (isRecording)
        {
            recordButton.image.color = new Color(1, 0, 0);
            recordText.text = "記録中";
        }
        else
        {
            recordText.text = "記録開始";

            recordButton.image.color = new Color(0, 0, 1);

            // ファイルへ書き込みます。
            // 出力場所は、Unityのプロジェクト直下です。
            File.WriteAllLines("WatchData/加速度_" + writeIndex + ".txt", accList.ToArray());
            File.WriteAllLines("WatchData/ジャイロ_" + writeIndex + ".txt", gyroList.ToArray());
            writeIndex++;

            accList.Clear();
            gyroList.Clear();

            Debugger.Log("Watch情報を書き込みました。");
        }
#endif

    }

    void Update()
    {

#if UNITY_EDITOR

        if (!isRecording) return;

        accList.Add(WatchManager.Instance.Acc.ToString());
        gyroList.Add(WatchManager.Instance.GyroAngle.ToString());
#endif

    }

}
