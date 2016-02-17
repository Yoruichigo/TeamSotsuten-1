///
/// チュートリアルで使用するgoodイメージ用スクリプト
/// code by ogata
///


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialGood : MonoBehaviour {


    [SerializeField]
    GameObject Image_Good;


    const string TweenNameStart = "TutorialGoodStart";

    const string TweenNameEnd = "TutorialGoodEnd";


    Action SubFunc = null;

	// Use this for initialization
	void Start () {
        Image_Good.SetActive(false);
        SubFunc = null;
	}


    void OffFunc()
    {
        if (!uTween.IsPlaying(TweenNameEnd))
        {
            Image_Good.SetActive(false);
            SubFunc = null;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (null != SubFunc)
        {
            SubFunc();
        }

        switch(TutorialSequence.GetNowGoodState())
        {
            case TutorialSequence.GoodState.ON:
                Image_Good.SetActive(true);
                TweenPlay(TweenNameStart);
                break;
            case TutorialSequence.GoodState.UPDATE:
                break;
            case TutorialSequence.GoodState.OFF:
                TweenPlay(TweenNameEnd);
                SubFunc = OffFunc;
                break;

            case TutorialSequence.GoodState.NULL:
                if (TutorialSequence.State.NULL == TutorialSequence.GetNowState()) {
                    Destroy(gameObject);
                }
                break;
        }
	}



    void TweenPlay(string _name)
    {
        var playlist = uTween.GetPlayList(_name);
        foreach (var dat in playlist)
        {
            dat.Play();
        }
    }





}
