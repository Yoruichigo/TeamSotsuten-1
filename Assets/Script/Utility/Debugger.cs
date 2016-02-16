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
    static GameObject debugFrame = null;
    static Text debugText = null;
    static int messageNum = 0;

    void Awake()
    {
        var child = transform.GetComponentInChildren<Canvas>();

        if (Global.IsBuidEditor())
        {
            debugText = child.GetComponentInChildren<Text>();
            Reset();
        }
        else
        {
            debugFrame = child.transform.FindChild("DebugFrame").gameObject;
            debugFrame.SetActive(false);
        }

    }


    /// <summary>
    /// デバッグ用のテキストをリセットする。
    /// </summary>
    public static void Reset()
    {
        if (!Global.IsBuidEditor()) return;

        debugText.text = "";
    }

    /// <summary>
    /// ログにメッセージを表示させる。
    /// </summary>
    /// <param name="message"></param>
    public static void Log(object message)
    { 
        if (!Global.IsBuidEditor()) return;

        Debug.Log(message);

        OverLineTextReset();

        debugText.text += message + "\n";

        messageNum++;


    }

    /// <summary>
    /// ログにエラーを表示させる。
    /// </summary>
    /// <param name="message"></param>
    public static void LogError(object message)
    {
        if (!Global.IsBuidEditor()) return;

        Debug.LogError(message);

        OverLineTextReset();

        debugText.text += "<color=red>" + message + "</color>" + "\n";

        messageNum++;
    }

    /// <summary>
    /// ログに警告を表示させる。
    /// </summary>
    /// <param name="message"></param>
    public static void LogWarning(object message)
    {
        if (!Global.IsBuidEditor()) return;

        Debug.LogWarning(message);

        OverLineTextReset();

        debugText.text += "<color=yellow>" + message + "</color>" + "\n";

        messageNum++;
    }


    private static void OverLineTextReset()
    {        
        if (!Global.IsBuidEditor()) return;

        const int MaxLineNum = 25;

        if (messageNum >= MaxLineNum)
        {
            messageNum = 0;

            debugText.text = "";
        }
    }
}
