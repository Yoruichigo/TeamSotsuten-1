﻿using UnityEngine;
using System.Collections;

public class uTweenSample : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        string playName = "test";


        if (Input.GetKeyDown(KeyCode.Q))
        {
            var move = uTween.Play(ref playName) as uTweenMove;
            move.startPosition = new Vector3(-100, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            uTween.Play(ref playName) ;
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
