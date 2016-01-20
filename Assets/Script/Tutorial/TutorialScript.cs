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
    GameObject AttackInfoObj;

    [SerializeField]
    GameObject GameRuleInfoObj;

    [SerializeField]
    GameObject CenterPositionObj;

    //アップデート用
    private Action NowFunc = null;

    //時間制御用
    int StartTime;


    //終了時間
    [SerializeField]
    int ATTACK_INFO_END_TIME = 3;

    [SerializeField]
    int GAMERULE_INFO_END_TIME = 3;

    //Ease時間（速度）
    [SerializeField]
    float ATTACK_INFO_EASE_TIME_MINUTE = 1.0f;

    [SerializeField]
    float GAMERULE_INFO_EASE_TIME_MINUTE = 1.0f;


    //Easeタイプ
    [SerializeField]
    iTween.EaseType ATTACK_INFO_EASE_TYPE = iTween.EaseType.easeOutCubic;

    [SerializeField]
    iTween.EaseType GAMERULE_INFO_EASE_TYPE = iTween.EaseType.easeOutCubic;

    //tweenフラグ
    bool IsTweenEnd_Attack = false;
    bool IsTweenEnd_GameRule = false;

    bool IsiTweenMoving = false;
    


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


    //アップデートを遅延させて行わせるためだけの変数、他人は弄らないでね
    int UpdateDelay_NowTime = 0;
    const int UPDATE_DELAY_TIME = 5;

	/// <summary>
    /// 初期化
    /// </summary>
    void Start () {
        //AttackInfoObj.SetActive(true);
        //GameRuleInfoObj.SetActive(false);
        
        //NowFuncに関数を突っ込む
        NowFunc = DelayFunc;

        TimeUpdate();
    }
	

    /// <summary>
    /// UIルートの座標が頭おかしいんで遅延して通常のアップデートを行う
    /// </summary>
    void DelayFunc()
    {
        if (UPDATE_DELAY_TIME < UpdateDelay_NowTime)
        {
            NowFunc = AttackInfoUpdate;
        }
        else
        {
            UpdateDelay_NowTime += 1;
        }
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
            AttackInfoObj.SetActive(false);
            GameRuleInfoObj.SetActive(false);
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

        //いーじんぐする
        if (IsTweenEnd_Attack == false) {
            IsTweenEnd_Attack = true;
            MoveObject(
                AttackInfoObj,
                CenterPositionObj.transform.position,
                ATTACK_INFO_EASE_TIME_MINUTE,
                ATTACK_INFO_EASE_TYPE);
        }

        //イージング後、イージングが終了したら
        if (IsTweenEnd_Attack && !IsiTweenMoving)
        {
        }


        //？の条件で次の状態へ
        if (GetNowMinute() > (StartTime + ATTACK_INFO_END_TIME) && !IsiTweenMoving)
        {
            //次のアプデ関数を突っ込む
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

        //いーじんぐする
        if (IsTweenEnd_GameRule == false && !IsiTweenMoving)
        {
            IsTweenEnd_GameRule = true;
            MoveObject(
                GameRuleInfoObj,
                CenterPositionObj.transform.position,
                GAMERULE_INFO_EASE_TIME_MINUTE,
                GAMERULE_INFO_EASE_TYPE);
        }

        //イージング後、イージングが終了したら
        if (IsTweenEnd_GameRule && !IsiTweenMoving)
        {
            AttackInfoObj.SetActive(false);
        }

        //？の条件で終了
        if (GetNowMinute() > (StartTime + GAMERULE_INFO_END_TIME) && !IsiTweenMoving)
        {
            GameRuleInfoObj.SetActive(false);
            NowFunc = null;
            state = State.NONE;
        }

    }

    /// <summary>
    /// moveObjectをITweenで動かす
    /// </summary>
    void MoveObject(GameObject moveObject, Vector3 targetPosition, float endMinuteTime, iTween.EaseType easeType)
    {
        //　targetPositionに向かって等速で移動
        Debug.Log(moveObject.transform.position);
        iTween.MoveTo(moveObject,
            iTween.Hash(
            "position", targetPosition,
            "time", endMinuteTime,
            "easetype", easeType,
            "oncomplete", "OnCompleteiTween",
            "oncompletetarget", this.gameObject
            ));
    }



    /// <summary>
    /// 制御用の変数を更新するだけ
    /// </summary>
    void TimeUpdate()
    {
        StartTime = GetNowMinute();
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


    /// <summary>
    /// iTweenの処理が終わったら来てもらう
    /// </summary>
    void OnCompleteiTween()
    {
        IsiTweenMoving = false;
    }


}
