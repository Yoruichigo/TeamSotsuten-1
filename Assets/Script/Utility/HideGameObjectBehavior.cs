//-------------------------------------------------------------
//  ゲームオブジェクトを隠すクラス
// 
//  code by m_yamada
//-------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class HideGameObjectBehavior : MonoBehaviour {

    [SerializeField]
    GameObject[] hidenGameObject = new GameObject[1];

    [SerializeField]
    MonoBehaviour[] hidenBehaviour = new MonoBehaviour[1];

	void Awake ()
    {

#if !UNITY_EDITOR
        if (!ConnectionManager.IsWacth) return;

        // ここでゲームオブジェクトを非アクティブにしている。
        for (int i = 0; i < hidenGameObject.Length; i++)
        {
            hidenGameObject[i].SetActive(false);
        }

        for (int i = 0; i < hidenBehaviour.Length; i++)
        {
            hidenBehaviour[i].enabled = false;
        }

        Debugger.Log(">> HideGameObjectBehavior");
#endif

    }
	
}
