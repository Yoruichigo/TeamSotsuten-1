/// 
///  チュートリアル画像表示スクリプト
///
///　code by ogata
///


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialScript : MonoBehaviour {

    
    // チュートリアルかどうか
    static bool state = true;
    static public bool IsTutorial { get { return state; } }


    //アップデート用
    private Action NowFunc = null;
    

	/// <summary>
    /// 初期化
    /// </summary>
    void Start () {

        //NowFuncに関数を突っ込む
        //NowFunc = DelayFunc;
        
    }
	

    /// <summary>
    /// アプデ
    /// </summary>
	void Update () {

        //現在入っている関数を回す（スイッチ文とか面倒だった）
        if (IsTutorial)
        {
            NowFunc();
        }
        else
        {
            state = false;
            NowFunc = null;
            Destroy(gameObject);
        }

    }
    
    
    /// <summary>
    /// 現在時刻を秒で取得
    /// </summary>
    /// <returns>MinuteTime</returns>
    int GetNowMinute()
    {
        int retdata = DateTime.Now.Second;
        retdata += DateTime.Now.Minute * 60;
        retdata += DateTime.Now.Hour * 60 * 60;
        return retdata;
    }
    

}
