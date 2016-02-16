using UnityEngine;
using System.Collections;

public class Global
{
    static public bool IsBuidEditor()
    {
        return Application.platform == RuntimePlatform.WindowsEditor || 
            Application.platform == RuntimePlatform.OSXEditor;
    }

    static public bool IsBuidAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }

    static public bool IsBuidIPhone()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
