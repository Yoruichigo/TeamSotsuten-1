/// 
///  チュートリアルのシーケンス管理クラス
///
///　code by ogata
///



using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialSequence : MonoBehaviour
{
    [SerializeField]
    int StartGuidTime = 10;

    [SerializeField]
    int WeakEndTime = 200;

    [SerializeField]
    int StrengthEndTime = 200;

    [SerializeField]
    int FinishGuidEndTime = 30;

    [SerializeField]
    int GoodActiveTime = 20;

    [SerializeField]
    int FinishWaitTime = 10;

    /// <summary>
    /// Good画像を表示し、次の状態へ行きます。
    /// </summary>
    static void MakeGood()
    {
        if (GoodState.NULL == nowGoodState) {
            nowGoodState = GoodState.SETUP;
        }
    }




    public enum State
    {
        NULL,
        START,
        ON_START_GUID,
        START_GUID,
        OUT_START_GUID,
        ON_WEAK,
        WEAK,
        OUT_WEAK,
        ON_STRENGTH,
        STRENGTH,
        OUT_STRENGTH,
        ON_FINISH_GUID,
        FINISH_GUID,
        OUT_FINISH_GUID,
        FINISH_WAIT,
        FINISH,
    }


    public enum GoodState
    {
        NULL,
        SETUP,
        ON,
        UPDATE,
        OFF,
    }


    static State nowState = State.NULL;
    static GoodState nowGoodState = GoodState.NULL;

    //チュートリアルかどうか(true = チュート中, false = チュート終了)
    static public bool IsTutorial {
        get {
            if (nowState != State.NULL) return true;
            return false;
        }
    }

    static public State GetNowState() { return nowState; }

    static public GoodState GetNowGoodState() { return nowGoodState; }



    int saveTime = 0;

    bool isStartWeakEndTime;
    bool isStartStrengthEndTime;

    // Use this for initialization
    //毎回の初期化処理
    public void Start()
    {
        nowState = State.START;
        nowGoodState = GoodState.NULL;
        isStartWeakEndTime = false;
        isStartStrengthEndTime = false;
    }

    // Update is called once per frame
    //更新
    public void Update()
    {
        
        switch (nowState)
        {
            default:
                Debug.LogError("Tutorial Sequence default in = " + nowState);
                Debugger.LogError("Tutorial Sequence default in = " + nowState);
                break;
            case State.NULL:
                gameObject.SetActive(false);
                break;
            case State.START:
                nowState = State.ON_START_GUID;
                break;
            case State.ON_START_GUID:
                nowState = State.START_GUID;
                saveTime = GetNowTime();
                break;
            case State.START_GUID:
                if (StartGuidEndCheck())
                {
                    nowState = State.OUT_START_GUID;
                }
                break;
            case State.OUT_START_GUID:
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
                else
                {
                    if (MotionManager.MotionSkillType.NONE != MotionManager.Instance.MotionSkill)
                    {
                        PlayerAttackEffectManager.Instance.CheckType(MotionManager.MotionSkillType.WEAK);
                    }
                }
                break;
            case State.OUT_WEAK:
                nowState = State.ON_STRENGTH;
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
                else
                {
                    if (MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill)
                    {
                        PlayerAttackEffectManager.Instance.CheckType(MotionManager.MotionSkillType.STRENGTH);
                    }
                }
                break;
            case State.OUT_STRENGTH:
                nowState = State.ON_FINISH_GUID;
                break;
            case State.ON_FINISH_GUID:
                saveTime = GetNowTime();
                nowState = State.FINISH_GUID;
                break;
            case State.FINISH_GUID:
                if (FinishGuidEndCheck())
                {
                    nowState = State.OUT_FINISH_GUID;
                }
                break;
            case State.OUT_FINISH_GUID:
                nowState = State.FINISH_WAIT;
                saveTime = GetNowTime();
                break;
            case State.FINISH_WAIT:
                if (FinishWaitEndCheck())
                {
                    nowState = State.FINISH;
                }
                break;
            case State.FINISH:
                var playList = uTween.GetPlayList("UITimeAppearance");
                for (int i = 0; i < playList.Length; i++)
                {
                    playList[i].Play();
                }

                nowState = State.NULL;
                break;
        }

        switch (nowGoodState)
        {
            case GoodState.SETUP:
                nowGoodState = GoodState.ON;
                break;
            case GoodState.ON:
                saveTime = GetNowTime();
                nowGoodState = GoodState.UPDATE;
                break;
            case GoodState.UPDATE:
                if (GoodEndCheck())
                {
                    nowGoodState = GoodState.OFF;
                }
                break;
            case GoodState.OFF:
                nowGoodState = GoodState.NULL;
                break;
        }

    }

    bool StartGuidEndCheck()
    {
        if (GetNowTime() > (saveTime + StartGuidTime))
        {
            return true;
        }

        return false;
    }



    bool WeakEndCheck()
    {
        if (!isStartWeakEndTime)
        {
            if (MotionManager.MotionSkillType.NONE != MotionManager.Instance.MotionSkill)
            {
                isStartWeakEndTime = true;
                saveTime = GetNowTime();
                MakeGood();
            }
            return false;
        }

        if (GetNowTime() > (saveTime + WeakEndTime))
        {
            return true;   
        }

        return false;
    }


    bool StrengthEndCheck()
    {
        if (!isStartStrengthEndTime)
        {
            if (MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill)
            {
                isStartStrengthEndTime = true;
                saveTime = GetNowTime();
                MakeGood();
            }
            return false;
        }

        if (GetNowTime() > (saveTime + StrengthEndTime))
        {
            return true;
        }

        return false;
    }


    bool FinishGuidEndCheck()
    {
        if (GetNowTime() > (saveTime + FinishGuidEndTime))
        {
            return true;
        }

        return false;
    }

    bool FinishWaitEndCheck()
    {
        if (GetNowTime() > (saveTime + FinishWaitTime))
        {
            return true;
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
