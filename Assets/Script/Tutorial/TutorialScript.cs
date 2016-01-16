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

    [SerializeField]
    GameObject AttackInfo;

    [SerializeField]
    GameObject GameRuleInfo;

    //アップデート用
    private Action NowFunc = null;

    //時間制御用
    int StartTime;

    [SerializeField]
    const int ATTACK_INFO_END_TIME = 3;

    [SerializeField]
    const int GAMERULE_INFO_END_TIME = 3;

    
	/// <summary>
    /// 初期化
    /// </summary>
    void Start () {
        
        AttackInfo.SetActive(true);
        GameRuleInfo.SetActive(false);

        //NowFuncに関数を突っ込む
        NowFunc = AttackInfoUpdate;

        //enableの使い方サンプル
        //GetComponent<ClientEnemyAttack>().enabled = false;

        TimeUpdate();
    }
	

    /// <summary>
    /// アプデ
    /// </summary>
	void Update () {

        //現在入っている関数を回す（スイッチ文とか面倒だった）
        if (NowFunc != null) {
            NowFunc();
        }
        
    }


    /// <summary>
    /// 攻撃の仕方説明(1枚目)の制御
    /// </summary>
    void AttackInfoUpdate()
    {

        //？の条件で次の状態へ
        if (GetNowMinute() > (StartTime + ATTACK_INFO_END_TIME))
        {
            AttackInfo.SetActive(false);
            GameRuleInfo.SetActive(true);

            //関数を突っ込む
            NowFunc = GameRuleInfoUpdate;

            TimeUpdate();
        }
    }

    /// <summary>
    /// ゲームの流れ説明(2枚目)の制御
    /// </summary>
    void GameRuleInfoUpdate()
    {
        //？の条件で終了
        if (GetNowMinute() > (StartTime + GAMERULE_INFO_END_TIME))
        {
            GameRuleInfo.SetActive(false);
            NowFunc = null;
        }

    }


    void TimeUpdate()
    {
        StartTime = GetNowMinute();
    }
    
    // 現在時刻を秒で取得
    int GetNowMinute()
    {
        int retdata = DateTime.Now.Second;
        retdata += DateTime.Now.Minute * 60;
        retdata += DateTime.Now.Hour * 60 * 60;
        return retdata;
    }

}
