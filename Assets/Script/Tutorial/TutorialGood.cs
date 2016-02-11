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
	    
        switch(TutorialManager.Instance.GetNowState())
        {
            case TutorialManager.State.ON_GOOD:
                Image_Good.SetActive(true);
                break;
            case TutorialManager.State.GOOD:
                break;
            case TutorialManager.State.OUT_GOOD:
                Image_Good.SetActive(false);
                break;

            case TutorialManager.State.FINISH:
                Destroy(gameObject);
                break;
        }
	}



}
