//-------------------------------------------------------------
//  デバッグ機能をラッピングしたスクリプト
//
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Debugger : MonoBehaviour 
{

    static Text debugText = null;
    static int messageNum = 0;

    void Awake()
    {
//#if DEBUG
        var child = transform.GetComponentInChildren<Canvas>();

        debugText = child.GetComponentInChildren<Text>();

        Reset();
//#endif
    }


    /// <summary>
    /// デバッグ用のテキストをリセットする。
    /// </summary>
    public static void Reset()
    {
#if DEBUG
        debugText.text = "";
#endif
    }

    /// <summary>
    /// ログにメッセージを表示させる。
    /// </summary>
    /// <param name="message"></param>
    public static void Log(object message)
    { 
#if UNITY_EDITOR
        Debug.Log(message);
//#endif
        OverLineTextReset();

        debugText.text += message + "\n";

        messageNum++;
#endif

    }

    /// <summary>
    /// ログにエラーを表示させる。
    /// </summary>
    /// <param name="message"></param>
    public static void LogError(object message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
//#endif

        OverLineTextReset();

        debugText.text += "<color=red>" + message + "</color>" + "\n";

        messageNum++;
#endif
    }

    /// <summary>
    /// ログに警告を表示させる。
    /// </summary>
    /// <param name="message"></param>
    public static void LogWarning(object message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
//#endif
        OverLineTextReset();

        debugText.text += "<color=yellow>" + message + "</color>" + "\n";

        messageNum++;
#endif
    }


    private static void OverLineTextReset()
    {
#if UNITY_EDITOR
        const int MaxLineNum = 25;

        if (messageNum >= MaxLineNum)
        {
            messageNum = 0;

            debugText.text = "";
        }
#endif
    }
}
