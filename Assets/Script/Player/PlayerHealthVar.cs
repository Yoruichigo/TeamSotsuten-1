using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthVar : MonoBehaviour 
{
    [SerializeField]
    Image helthVar = null;

    int lifeMax = 0;    // 体力の最大

    void Start()
    {
        lifeMax = GameManager.Instance.GetPlayerData().HelthPoint;
        helthVar.fillAmount = 1;
        helthVar.enabled = true;
    }

    void Update()
    {
        helthVar.fillAmount = (float)GameManager.Instance.GetPlayerData().HelthPoint / (float)lifeMax;
    }

}
