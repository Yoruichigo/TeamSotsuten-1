using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultWindowInfo : MonoBehaviour 
{
    [SerializeField]
    Image gameOverImage = null;

    [SerializeField]
    Image gameClearImage = null;

	// Use this for initialization
	void Start () 
    {
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
	
}
