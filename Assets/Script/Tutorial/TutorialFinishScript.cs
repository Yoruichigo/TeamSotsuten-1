using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialFinishScript : MonoBehaviour {


    [SerializeField]
    GameObject ImageObj;


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
	
	// Update is called once per frame
	void Update () {

        switch (TutorialSequence.GetNowState())
        {
            case TutorialSequence.State.ON_FINISH_GUID:
                ImageObj.SetActive(true);
                uTween.Play(tweenNameIn);
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
