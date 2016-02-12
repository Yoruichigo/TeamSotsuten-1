using UnityEngine;
using System.Collections;

public class TutorialGuid : MonoBehaviour {

    [SerializeField]
    GameObject GuidImage;

    [SerializeField]
    int SlideWaitTime = 10;

    string tweenName1 = "TutorialGuid1";
    string tweenName3 = "TutorialGuid3";
   
    // Use this for initialization
    void Start () {
        GuidImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (TutorialSequence.GetNowState())
        {
            case TutorialSequence.State.ON_START_GUID:
                GuidImage.SetActive(true);
                uTween.Play(tweenName1);
                break;

            case TutorialSequence.State.START_GUID:
                break;
            case TutorialSequence.State.OUT_START_GUID:

                break;
            case TutorialSequence.State.ON_FINISH_GUID:
                uTween.Play(tweenName3);
                break;
            case TutorialSequence.State.FINISH_GUID:
                if (!uTween.IsPlaying(tweenName3))
                {
                    GuidImage.SetActive(false);
                }
                break;
            case TutorialSequence.State.FINISH:
                Destroy(gameObject);
                break;
        }

    }

}
