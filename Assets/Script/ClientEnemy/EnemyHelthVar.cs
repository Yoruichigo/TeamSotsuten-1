using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class EnemyHelthVar : MonoBehaviour 
{
    [SerializeField]
    Image helthVar = null;

    int lifeMax = 0;    // 体力

	// Use this for initialization
	void Start () 
    {

	}

    public void SetMaxLife(int maxLife)
    {
        lifeMax = maxLife;
    }

	// Update is called once per frame
	void Update () 
    {
        if (EnemyManager.Instance.IsEnemyNothing) return;

        helthVar.enabled = EnemyManager.Instance.GetActiveEnemyData().IsActive();

        if (helthVar.enabled)
        {
            helthVar.fillAmount = (float)EnemyManager.Instance.GetActiveEnemyData().Life / (float)lifeMax;
        }
	
	}
}
