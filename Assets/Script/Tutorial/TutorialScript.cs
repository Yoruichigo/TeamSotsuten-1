/// 
///  チュートリアル画像表示スクリプト
///
///　code by ogata
///


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialScript : MonoBehaviour
{

    // 誘導画像
    [SerializeField]
    GameObject Image_AttackInduction;

    // スライダー
    [SerializeField]
    GameObject Slider_AttackInduction;

    //tweenタイプ
    [SerializeField]
    iTween.EaseType SliderEaseType = iTween.EaseType.linear;

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

    //iTween有効フラグ
    private bool iTweenSliderActivate = false;

    const int WaitTime = 20;
    int SaveTime;


    string tweenNameSlider = "TutorialSlider";
    string tweenNameImage = "TutorialPleaseAttack";

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {

        Image_AttackInduction.SetActive(false);
        Slider_AttackInduction.SetActive(false);
        SaveTime = TutorialSequence.GetNowTime();
        OldSliderTweenEndTime = TutorialSequence.GetNowTime();

    }


    /// <summary>
    /// 終了関数
    /// </summary>
    void EndUpdate()
    {
        Image_AttackInduction.SetActive(false);
        Slider_AttackInduction.SetActive(false);
        Destroy(gameObject);
    }




    /// <summary>
    /// アプデ
    /// </summary>
	void Update()
    {

        switch (TutorialSequence.GetNowState())
        {
            case TutorialSequence.State.ON_WEAK:
                Image_AttackInduction.SetActive(true);
                Slider_AttackInduction.SetActive(true);
                SaveTime = TutorialSequence.GetNowTime();
                uTween.Play(tweenNameSlider);
                uTween.Play(tweenNameImage);
                break;
            case TutorialSequence.State.WEAK:
                SliderWaitAndTween(SLIDER_TIME_WEAK);
                break;
            case TutorialSequence.State.OUT_WEAK:
                SlideriTweenStop();
                TutorialSequence.MakeGood();
                break;
            case TutorialSequence.State.ON_STRENGTH:
                SaveTime = TutorialSequence.GetNowTime();
                break;
            case TutorialSequence.State.STRENGTH:
                SliderWaitAndTween(SLIDER_TIME_STRENGTH);
                break;
            case TutorialSequence.State.OUT_STRENGTH:
                SlideriTweenStop();
                TutorialSequence.MakeGood();
                break;
            case TutorialSequence.State.FINISH:
                EndUpdate();
                break;
        }

    }



    //待機時間をつけたスライダー動作
    void SliderWaitAndTween(float _time)
    {
        if (!iTweenSliderActivate &&
            (OldSliderTweenEndTime + SLIDER_WAIT_TIME) < TutorialSequence.GetNowTime())
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
                "easetype", SliderEaseType,
                "onUpdate", "iTween_SliderUpdate",
                "oncomplete", "iTween_SliderEnd",
                "oncompletetarget", this.gameObject));

    }


    void SlideriTweenStop()
    {
        OldSliderTweenEndTime = TutorialSequence.GetNowTime();
        iTweenSliderActivate = false;
        iTween.Stop(gameObject);

        //Slider_AttackInduction.value = 0;
        var sliderobj = Slider_AttackInduction.GetComponent<Slider>();
        sliderobj.value = 0;
    }


    // iTweenで来るupdate関数
    void iTween_SliderUpdate(float value)
    {
        //Slider_AttackInduction.value = value;
        var sliderobj = Slider_AttackInduction.GetComponent<Slider>();
        sliderobj.value = value;
    }

    // iTweenが終了したとき来る関数
    void iTween_SliderEnd()
    {
        //Slider_AttackInduction.value = 0;
        var sliderobj = Slider_AttackInduction.GetComponent<Slider>();
        sliderobj.value = 0;

        OldSliderTweenEndTime = TutorialSequence.GetNowTime();
        iTweenSliderActivate = false;
    }






}







