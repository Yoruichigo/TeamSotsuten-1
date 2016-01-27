/*
    確認用デバッグログです。
    
    code by ogata
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugText1Script : MonoBehaviour {

    [SerializeField]
    Transform TargetTransform = null;

    [SerializeField]
    string HeadText = "null";

    Text DebugText = null;
    
	// Use this for initialization
	void Start () {
        DebugText = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        string textData;
        
        textData = HeadText + " x/" + TargetTransform.position.x + " y/" + TargetTransform.position.y + "z/" + TargetTransform.position.z;

        Debugger.Log(textData);
        DebugText.text = textData;
    }
}
