using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialSliderImage : MonoBehaviour {

    [SerializeField]
    Sprite[] Icons;


	// Use this for initialization
	void Start () {

        if(PlayerManager.Instance.IsFencer){
            gameObject.GetComponent<Image>().sprite = Icons[0];
        }
        else if (PlayerManager.Instance.IsMagician)
        {
            gameObject.GetComponent<Image>().sprite = Icons[1];
        }
        Destroy(this);
	}

}
