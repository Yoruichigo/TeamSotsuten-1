/// 
///  チュートリアルのシーケンス管理クラス
///
///　code by ogata
///



using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    int WeakMosionStartWaitTime = 3;

    [SerializeField]
    int StrengthMosionStartWaitTime = 3;





    public enum State
    {
        NULL,
        ON_START_GUID,
        START_GUID,
        ON_WEAK,
        WEAK,
        OUT_WEAK,
        ON_STRENGTH,
        STRENGTH,
        OUT_STRENGTH,
        ON_FINISH_GUID,
        FINISH_GUID,
        FINISH,
    }

    State nowState = State.NULL;

    //チュートリアルかどうか(true = チュート中, false = チュート終了)
    public bool IsTutorial { get {
            if (nowState != State.NULL) return true;
            return false;
        }
    }

    public State GetNowState(){ return nowState; }

    public void OutWeakEnd()
    {
        if (State.OUT_WEAK == nowState)
        {
            nowState = State.ON_STRENGTH;
        }
    }

    public void OutStrengthEnd()
    {
        if (State.OUT_STRENGTH == nowState)
        {
            nowState = State.ON_FINISH_GUID;
        }
    }


    int SaveTime = 0;

    public override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    //毎回の初期化処理
    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    //更新
    public override void Update()
    {
        base.Update();

        switch (nowState)
        {
            case State.ON_START_GUID:
                nowState = State.START_GUID;
                break;
            case State.START_GUID:
                nowState = State.ON_WEAK;
                break;
            case State.ON_WEAK:
                {
                    SaveTime = GetNowTime();
                    nowState = State.WEAK;
                }
                break;
            case State.WEAK:
                if (WeakEndCheck())
                {
                    nowState = State.OUT_WEAK;
                }
                break;
            case State.OUT_WEAK:

                break;
            case State.ON_STRENGTH:
                {
                    SaveTime = GetNowTime();
                    nowState = State.STRENGTH;
                }
                break;
            case State.STRENGTH:
                if (StrengthEndCheck())
                {
                    nowState = State.OUT_STRENGTH;
                }
                break;
            case State.OUT_STRENGTH:

                break;
            case State.ON_FINISH_GUID:
                nowState = State.FINISH_GUID;
                break;
            case State.FINISH_GUID:
                nowState = State.FINISH;
                break;
            case State.FINISH:
                nowState = State.NULL;
                break;

        }
    }



    bool WeakEndCheck()
    {
        if (GetNowTime() > (SaveTime + WeakMosionStartWaitTime))
        { 
            if (MotionManager.MotionSkillType.WEAK == MotionManager.Instance.MotionSkill)
            {
                return true;
            }
        }

        return false;
    }


    bool StrengthEndCheck()
    {
        if (GetNowTime() > (SaveTime + StrengthMosionStartWaitTime))
        {
            if (MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 現在時刻を秒で取得,１桁目のミリ秒から取得
    /// </summary>
    /// <returns>SecondTime</returns>
    public static int GetNowTime()
    {
        int retdata;
        retdata = DateTime.Now.Millisecond / 100;
        retdata += DateTime.Now.Second * 10;
        retdata += DateTime.Now.Minute * 10 * 60;
        retdata += DateTime.Now.Hour * 10 * 60 * 60;
        return retdata;
    }

}
