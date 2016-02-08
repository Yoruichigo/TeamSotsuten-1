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
    }

    void Update()
    {
#if !UNITY_EDITOR
        helthVar.enabled = Vuforia.VuforiaBehaviour.IsMarkerLookAt;

        if (helthVar.enabled)
        {
            HelthVarUpdate();
        }
#else
        HelthVarUpdate();
#endif

    }

    void HelthVarUpdate()
    {
        helthVar.fillAmount = (float)GameManager.Instance.GetPlayerData().HelthPoint / (float)lifeMax;
    }

}
