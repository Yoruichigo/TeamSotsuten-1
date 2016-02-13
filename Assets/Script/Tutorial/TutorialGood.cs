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
        

        switch(TutorialSequence.GetNowGoodState())
        {
            case TutorialSequence.GoodState.ON:
                Image_Good.SetActive(true);
                break;
            case TutorialSequence.GoodState.UPDATE:
                break;
            case TutorialSequence.GoodState.OFF:
                Image_Good.SetActive(false);
                break;

            case TutorialSequence.GoodState.NULL:
                if (TutorialSequence.State.NULL == TutorialSequence.GetNowState()) {
                    Destroy(gameObject);
                }
                break;
        }
	}
    


}
