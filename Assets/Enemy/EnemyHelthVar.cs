using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class EnemyHelthVar : MonoBehaviour 
{
    [SerializeField]
    EnemyData TargetEnamy;      //敵

    Image HelthVar;

    int lifeMaX;//体力

	// Use this for initialization
	void Start () 
    {
        lifeMaX = TargetEnamy.Life;
        HelthVar = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (TargetEnamy.IsActive())
        {
            HelthVar.enabled = true;
            HelthVar.fillAmount = TargetEnamy.Life / lifeMaX;
        }
        else 
        {
            HelthVar.enabled = false;
        }
	
	}
}
