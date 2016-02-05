using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthVar : MonoBehaviour 
{
    Slider helthVar;

    int lifeMax;//体力の最大

    // Use this for initialization
    void Start()
    {
        lifeMax = GameManager.Instance.GetPlayerData().HelthPoint;
        helthVar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        if (Vuforia.VuforiaBehaviour.IsMarkerLookAt)
        {
            helthVar.enabled = true;
            helthVar.value = (float)GameManager.Instance.GetPlayerData().HelthPoint / (float)lifeMax;
        }
        else
        {
            helthVar.enabled = false;
        }
#else
        helthVar.enabled = true;
        helthVar.value = (float)GameManager.Instance.GetPlayerData().HelthPoint / (float)lifeMax;
#endif


    }

}
