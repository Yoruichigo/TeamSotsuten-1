using UnityEngine;
using System.Collections;

public class NumberImageRendererSample : MonoBehaviour {

    [SerializeField]
    NumberImageRenderer sample = null;

    int time = 60 * 3 * 60;
	
    // Use this for initialization
    void Start()
    {
    }

	// Update is called once per frame
	void Update () {
        time -= 1;
        sample.Rendering(time / 60);
	}
}
