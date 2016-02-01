using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        string playName = "test";

        if (Input.GetKeyDown(KeyCode.A))
        {
            uTween.Play(ref playName);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            uTween.Pause(ref playName);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            uTween.Resume(ref playName);
        }
    }
}
