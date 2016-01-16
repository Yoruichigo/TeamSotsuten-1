using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthVar : MonoBehaviour 
{
    Slider HelthVar;

    int lifeMax;//体力の最大

    // Use this for initialization
    void Start()
    {
        lifeMax = GameManager.Instance.GetPlayerData().HelthPoint;
        HelthVar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vuforia.VuforiaBehaviour.IsMarkerLookAt)
        {
            HelthVar.enabled = true;
            HelthVar.value = (float)GameManager.Instance.GetPlayerData().HelthPoint / (float)lifeMax;

            Debugger.Log("割合 = " + HelthVar.value);
        }
        else
        {
            HelthVar.enabled = false;
        }

    }

}
