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

    enum State
    { 
        NONE,
        ATTACK,     //< 攻撃説明
        GAME_RULE   //< ゲームルール
    }

    static State state = State.ATTACK;

    // チュートリアルかどうか
    static public bool IsTutorial { get { return state != State.NONE; } }

    static public bool IsTutorialGameRule { get { return state == State.GAME_RULE; } }

    static public bool IsTutorialAttack { get { return state == State.ATTACK; } }
    
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
        if (IsTutorial)
        {
            NowFunc();
        }
        else
        {
            AttackInfo.SetActive(false);
            GameRuleInfo.SetActive(false);
            NowFunc = null;
            Destroy(gameObject);
        }

    }


    /// <summary>
    /// 攻撃の仕方説明(1枚目)の制御
    /// </summary>
    void AttackInfoUpdate()
    {
        state = State.ATTACK;

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
        state = State.GAME_RULE;

        //？の条件で終了
        if (GetNowMinute() > (StartTime + GAMERULE_INFO_END_TIME))
        {
            GameRuleInfo.SetActive(false);
            NowFunc = null;
            state = State.NONE;
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
