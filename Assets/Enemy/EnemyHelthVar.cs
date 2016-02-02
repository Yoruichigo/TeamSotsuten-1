using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class EnemyHelthVar : MonoBehaviour 
{
    Slider helthVar;

    int lifeMaX;//体力

	// Use this for initialization
	void Start () 
    {
        helthVar = GetComponent<Slider>();
	}

    public void SetMaxLife(int maxLife)
    {
        lifeMaX = maxLife;
    }

	// Update is called once per frame
	void Update () 
    {
        if (helthVar == null) return;

        if (EnemyManager.Instance.GetActiveEnemyData().IsActive())
        {
            helthVar.enabled = true;
            helthVar.value = (float)EnemyManager.Instance.GetActiveEnemyData().Life / (float)lifeMaX;
        }
        else 
        {
            helthVar.enabled = false;
        }
	
	}
}
