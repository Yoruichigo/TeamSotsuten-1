using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class EnemyHelthVar : MonoBehaviour 
{
    Image HelthVar;

    int lifeMaX;//体力

	// Use this for initialization
	void Start () 
    {
        SetMaxLife();
        HelthVar = GetComponent<Image>();
	}

    void SetMaxLife()
    {
        lifeMaX = EnemyManager.Instance.GetActiveEnemyData().Life;
    }

	// Update is called once per frame
	void Update () 
    {

        if (EnemyManager.Instance.GetActiveEnemyData().IsActive())
        {
            HelthVar.enabled = true;
            HelthVar.fillAmount = EnemyManager.Instance.GetActiveEnemyData().Life / lifeMaX;
        }
        else 
        {
            HelthVar.enabled = false;
        }
	
	}
}
