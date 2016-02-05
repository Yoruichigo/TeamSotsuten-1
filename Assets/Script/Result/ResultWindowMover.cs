using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultWindowMover : MonoBehaviour 
{
    [SerializeField]
    Image gameOverImage = null;

    [SerializeField]
    Image gameClearImage = null;

	// Use this for initialization
	void Start () 
    {
        iTween.RotateTo(this.gameObject, iTween.Hash("x", 180, "time", 3));

        if (GameManager.IsGameClear)
        {
            gameOverImage.gameObject.SetActive(false);
            gameClearImage.gameObject.SetActive(true);
            return;
        }

        if (GameManager.IsGameOver)
        {
            gameOverImage.gameObject.SetActive(true);
            gameClearImage.gameObject.SetActive(false);
            return;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
