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
        HelthVar = GetComponent<Image>();
	}

    public void SetMaxLife(int maxLife)
    {
        lifeMaX = maxLife;
    }

	// Update is called once per frame
	void Update () 
    {
        if (HelthVar == null) return;

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
