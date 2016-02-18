using UnityEngine;
using System.Collections;

public class LookAtMarker : MonoBehaviour {


    [SerializeField]
    GameObject ImageLookAt;

    uTweenBase IBcs;

    // Use this for initialization
    void Start () {

        //最初隠しておく
        ImageLookAt.SetActive(false);

        IBcs = ImageLookAt.GetComponent<uTweenBase>();
        IBcs.Play();

	}
	
	// Update is called once per frame
	void Update () {

        if (!DoingUpdate())
        {
            return;
        }
        
        switch (GameManager.Instance.GetLookState())
        {
            default:
                break;
            case GameManager.LookMarkerState.OnLook:
                IBcs.Resume();
                ImageLookAt.SetActive(false);
                break;

            case GameManager.LookMarkerState.OnNonLook:
                IBcs.Play();
                ImageLookAt.SetActive(true);
                break;
        }
        

    }

    /// <summary>
    /// アップデートしていいかチェック
    /// </summary>
    /// <returns></returns>
    bool DoingUpdate()
    {
        /*
        //　チュートリアル中なら無視
        if (TutorialSequence.IsTutorial)
        {
            return false;
        }
        */

        return true;
    }


}
