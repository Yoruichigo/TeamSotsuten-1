using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialFinishScript : MonoBehaviour {


    [SerializeField]
    GameObject ImageObj;

    [SerializeField]
    string endWaitTweenName = "";

    string tweenNameIn = "TutorialFinishIn";
    string tweenNameOut = "TutorialFinishOut";


    Action SubFunc = null;

	// Use this for initialization
	void Start () {
        SubFunc = null;
        ImageObj.SetActive(false);
	}

    void MyFinish()
    {
        if (!uTween.IsPlaying(tweenNameOut)) {
            ImageObj.SetActive(false);
            gameObject.SetActive(false);
            SubFunc = null;
        }
    }

    void DelayStartFunc()
    {
        if (!uTween.IsPlaying(endWaitTweenName))
        {
            uTween.Play(tweenNameIn);
            ImageObj.SetActive(true);
            SubFunc = null;
        }
    }

	
	// Update is called once per frame
	void Update () {

        if (null != SubFunc)
        {
            SubFunc();
        }

        switch (TutorialSequence.GetNowState())
        {
            case TutorialSequence.State.ON_FINISH_GUID:
                SubFunc = DelayStartFunc;
                break;
            case TutorialSequence.State.FINISH_GUID:

                break;
            case TutorialSequence.State.OUT_FINISH_GUID:
                uTween.Play(tweenNameOut);
                break;
            case TutorialSequence.State.FINISH:
                SubFunc = MyFinish;
                break;
        }
	
	}





}
