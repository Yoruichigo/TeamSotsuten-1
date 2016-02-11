﻿/// 
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
    int WeakMosionStartWaitTime = 10;

    [SerializeField]
    int StrengthMosionStartWaitTime = 10;

    [SerializeField]
    int GoodActiveTime = 20;


    /// <summary>
    /// Good画像を表示し、次の状態へ行きます。
    /// </summary>
    public void MakeGood()
    {
        switch(nowState)
        {
            default:
                Debugger.LogError("TutorialManager MakeGood NULL Error");
                goodNextState = State.NULL;
                break;
            case State.OUT_WEAK:
                goodNextState = State.ON_STRENGTH;
                break;
            case State.OUT_STRENGTH:
                goodNextState = State.ON_FINISH_GUID;
                break;
        }
        nowState = State.ON_GOOD;
    }


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
        ON_GOOD,
        GOOD,
        OUT_GOOD,
    }

    State nowState = State.NULL;

    State goodNextState = State.NULL;

    //チュートリアルかどうか(true = チュート中, false = チュート終了)
    public bool IsTutorial {
        get {
            if (nowState != State.NULL) return true;
            return false;
        }
    }

    public State GetNowState() { return nowState; }

    



    int saveTime = 0;

    public override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    //毎回の初期化処理
    public override void Start()
    {
        base.Start();
        nowState = State.ON_START_GUID;
    }

    // Update is called once per frame
    //更新
    public override void Update()
    {
        base.Update();
        Debug.Log("T_M " + nowState);
        
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
                    saveTime = GetNowTime();
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
                    saveTime = GetNowTime();
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
            case State.ON_GOOD:
                saveTime = GetNowTime();
                nowState = State.GOOD;
                break;
            case State.GOOD:
                if (GoodEndCheck())
                {
                    nowState = State.OUT_GOOD;
                }
                break;
            case State.OUT_GOOD:
                nowState = goodNextState;
                break;
        }

    }



    bool WeakEndCheck()
    {
        if (GetNowTime() > (saveTime + WeakMosionStartWaitTime))
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
        if (GetNowTime() > (saveTime + StrengthMosionStartWaitTime))
        {
            if (MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill)
            {
                return true;
            }
        }

        return false;
    }


    bool GoodEndCheck()
    {
        if (GetNowTime() > (saveTime + GoodActiveTime))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 現在時刻を秒で取得,１桁目=ミリ秒 2桁目～秒
    /// </summary>
    /// <returns>Millisecond</returns>
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