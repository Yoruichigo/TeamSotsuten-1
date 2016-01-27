using UnityEngine;
using System.Collections;

public class ResultWindowMover : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        iTween.RotateTo(this.gameObject, iTween.Hash("x", 180,"time", 3));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
