///
/// チュートリアルで使用するgoodイメージ用スクリプト
/// code by ogata
///



using UnityEngine;
using System.Collections;

public class TutorialGood : MonoBehaviour {


    [SerializeField]
    GameObject Image_Good;


	// Use this for initialization
	void Start () {
        Image_Good.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    
        switch(TutorialSequence.GetNowState())
        {
            case TutorialSequence.State.ON_GOOD:
                Image_Good.SetActive(true);
                break;
            case TutorialSequence.State.GOOD:
                break;
            case TutorialSequence.State.OUT_GOOD:
                Image_Good.SetActive(false);
                break;

            case TutorialSequence.State.FINISH:
                Destroy(gameObject);
                break;
        }
	}



}
