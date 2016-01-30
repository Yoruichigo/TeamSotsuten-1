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

    // スライダー
    [SerializeField]
    Slider Slider_AttackInduction;

    //スライダー速度　弱
    [SerializeField]
    float SLIDER_TIME_WEAK = 5;
    
    //スライダー速度　強
    [SerializeField]
    float SLIDER_TIME_STRENGTH = 2;

    //スライダーの待機時間
    [SerializeField]
    int SLIDER_WAIT_TIME = 2;

    //スライダー用　過去時間
    int OldSliderTweenEndTime;

    //アップデート用
    private Action NowFunc = null;
    
    //iTween有効フラグ
    private bool iTweenSliderActivate = false;

    const int WaitTime = 20;
    int SaveTime;



	/// <summary>
    /// 初期化
    /// </summary>
    void Start () {
        //NowFuncに関数を突っ込む
        NowFunc = WakeInductionUpdate;

        SaveTime = GetNowTime();
        OldSliderTweenEndTime = GetNowTime();

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
        //ツイーン
        SliderWaitAndTween(SLIDER_TIME_WEAK);


        //スマホかどうか見て、適当にやる
        bool endFlag = ConnectionManager.IsSmartPhone ?
            MotionManager.MotionSkillType.WEAK == MotionManager.Instance.MotionSkill :
            SaveTime + WaitTime < GetNowTime();
        
        if(endFlag)
        {
            SaveTime = GetNowTime();
            SlideriTweenStop();
            NowFunc = StrengthInductionUpdate;
        }
    }

    /// <summary>
    /// 強攻撃関数
    /// </summary>
    void StrengthInductionUpdate()
    {
        SliderWaitAndTween(SLIDER_TIME_STRENGTH);


        bool endFlag = ConnectionManager.IsSmartPhone ?
            MotionManager.MotionSkillType.STRENGTH == MotionManager.Instance.MotionSkill :
            SaveTime + WaitTime < GetNowTime();
        
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
    

    //待機時間をつけたスライダー動作
    void SliderWaitAndTween(float _time)
    {
        if (!iTweenSliderActivate &&
            (OldSliderTweenEndTime + SLIDER_WAIT_TIME) < GetNowTime())
        {
            SliderValueTo(_time);
        }
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
        OldSliderTweenEndTime = GetNowTime();
        iTweenSliderActivate = false;
        iTween.Stop(gameObject);
        Slider_AttackInduction.value = 0;
    }


    // iTweenで来るupdate関数
    void iTween_SliderUpdate(float value)
    {
        Slider_AttackInduction.value = value;
    }

    // iTweenが終了したとき来る関数
    void iTween_SliderEnd()
    {
        Slider_AttackInduction.value = 0;
        OldSliderTweenEndTime = GetNowTime();
        iTweenSliderActivate = false;
    }


    
    /// <summary>
    /// 現在時刻を秒で取得,１桁目のミリ秒から取得
    /// </summary>
    /// <returns>SecondTime</returns>
    int GetNowTime()
    {
        int retdata;
        retdata = DateTime.Now.Millisecond / 100;
        retdata += DateTime.Now.Second * 10;
        retdata += DateTime.Now.Minute * 10 * 60;
        retdata += DateTime.Now.Hour * 10 * 60 * 60;
        return retdata;
    }
    

}






