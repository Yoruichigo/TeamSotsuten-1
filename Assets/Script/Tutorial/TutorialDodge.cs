using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TutorialDodge : MonoBehaviour {


    [SerializeField]
    GameObject ImageObj;

    [SerializeField]
    string TweenNameStart = "";

    [SerializeField]
    string TweenNameEnd = "";

    Action SubFunc = null;



	// Use this for initialization
	void Start () {
        SubFunc = null;
        ImageObj.SetActive(false);
    }


    void OutUpdate()
    {
        if (!uTween.IsPlaying(TweenNameEnd))
        {
            ImageObj.SetActive(false);
            SubFunc = null;
        }

    }

	
	// Update is called once per frame
	void Update () {

        if (SubFunc != null)
        {
            SubFunc();
        }

        switch (TutorialSequence.GetNowState())
        {
            case TutorialSequence.State.ON_DODGE:
                ImageObj.SetActive(true);
                TweenPlay(TweenNameStart);
                break;
            case TutorialSequence.State.DODGE:
                
                break;
            case TutorialSequence.State.OUT_DODGE:
                TweenPlay(TweenNameEnd);
                SubFunc = OutUpdate;
                break;
            case TutorialSequence.State.FINISH:
                gameObject.SetActive(false);
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
