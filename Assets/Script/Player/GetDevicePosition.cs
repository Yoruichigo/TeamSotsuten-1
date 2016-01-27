///-------------------------------------------------------------------------
///
/// code by miyake yuji
///
/// デバイスの情報を取得し、プレイヤーの座標を取得する為のスクリプト
/// 
///-------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetDevicePosition : MonoBehaviour
{
    [SerializeField]
    Text debugText = null;

    /// <summary>
    /// Update メソッドが最初に呼び出される前のフレームで呼び出されます
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// 毎フレーム呼ばれる。
    /// </summary>
    void Update()
    {

        /// デバイスの座標を取得
        if (SequenceManager.Instance.IsBuildWatch)
        {
            transform.position = SequenceManager.Instance.SingleCamera.transform.position;
        }
        else
        {
            transform.position = SequenceManager.Instance.ARCamera.transform.position;
        }

#if UNITY_EDITOR
        debugText.text = "プレイヤー座標 : " + transform.position;
#endif
    }
}
