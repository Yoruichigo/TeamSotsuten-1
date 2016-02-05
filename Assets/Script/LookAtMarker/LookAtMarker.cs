using UnityEngine;
using System.Collections;

public class LookAtMarker : MonoBehaviour {


    [SerializeField]
    GameObject ImageLookAt;
    

    // Use this for initialization
    void Start () {

        //最初隠しておく
        ImageLookAt.SetActive(false);

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
                ImageLookAt.SetActive(false);
                break;

            case GameManager.LookMarkerState.OnNonLook:
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
        //　チュートリアル中なら無視
        if (TutorialScript.IsTutorial)
        {
            return false;
        }

        return true;
    }


}
