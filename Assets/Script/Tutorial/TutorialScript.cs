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


    // 誘導画像
    [SerializeField]
    GameObject Image_AttackInduction;

    [SerializeField]
    Slider Slider_AttackInduction;

    [SerializeField]
    float SLIDER_TIME_WEAK = 5;

    [SerializeField]
    float SLIDER_TIME_STRENGTH = 2;

    //アップデート用
    private Action NowFunc = null;

    //iTween有効フラグ
    private bool iTweenSliderActivate = false;

    const int WaitTime = 10;
    int SaveTime;

	/// <summary>
    /// 初期化
    /// </summary>
    void Start () {
        //NowFuncに関数を突っ込む
        NowFunc = WakeInductionUpdate;

        SaveTime = GetNowMinute();

        //MotionManager.MotionSkillType.STRENGTH;
        //MotionManager.MotionSkillType.WEAK;
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

    }


    /// <summary>
    /// 弱攻撃関数
    /// </summary>
    void WakeInductionUpdate()
    {
        if (!iTweenSliderActivate)
        {
            SliderValueTo(SLIDER_TIME_WEAK);
        }

        bool endFlag = ConnectionManager.IsSmartPhone ?
            MotionManager.MotionSkillType.WEAK == MotionManager.Instance.MotionSkill :
            SaveTime + WaitTime < GetNowMinute();

        //if (MotionManager.MotionSkillType.WEAK == MotionManager.Instance.MotionSkill)
        //if(SaveTime + WaitTime < GetNowMinute())
        if(endFlag)
        {
            SaveTime = GetNowMinute();
            SlideriTweenStop();
            NowFunc = StrengthInductionUpdate;
        }
    }

    /// <summary>
    /// 強攻撃関数
    /// </summary>
    void StrengthInductionUpdate()
    {
        if (!iTweenSliderActivate)
        {
            SliderValueTo(SLIDER_TIME_STRENGTH);
        }

        bool endFlag = ConnectionManager.IsSmartPhone ?
            MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill :
            SaveTime + WaitTime < GetNowMinute();

        //if (MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill)
        //if (SaveTime + WaitTime < GetNowMinute())
        if(endFlag)
        {
            NowFunc = EndUpdate;
        }
    }


    /// <summary>
    /// 終了関数
    /// </summary>
    void EndUpdate()
    {
        state = false;
        NowFunc = null;
        Destroy(gameObject);
    }
    

    /// <summary>
    /// スライダーをiTweenで動かします
    /// </summary>
    /// <param name="_time"></param>
    void SliderValueTo(float _time)
    {
        iTweenSliderActivate = true;
        iTween.ValueTo(gameObject,
            iTween.Hash(
                "from", 0,
                "to", 1,
                "time", _time,
                "onUpdate", "iTween_SliderUpdate", 
                "oncomplete", "iTween_SliderEnd",
                "oncompletetarget", this.gameObject));

    }


    void SlideriTweenStop()
    {
        iTweenSliderActivate = false;
        iTween.Stop(gameObject);
    }


    // iTweenで来るupdate関数
    void iTween_SliderUpdate(float value)
    {
        Slider_AttackInduction.value = value;
    }

    // iTweenが終了したとき来る関数
    void iTween_SliderEnd()
    {
        iTweenSliderActivate = false;
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






