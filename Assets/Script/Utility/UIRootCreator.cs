using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRootCreator : MonoBehaviour {

    [SerializeField]
    List<Canvas> uiRoot = new List<Canvas>();

    [SerializeField]
    Camera rightCamera = null;

    [SerializeField]
    Camera leftCamera = null;

	// Use this for initialization
	void Awake () 
    {
        for (int i = 0; i < uiRoot.Count; i++)
        {
            if (SequenceManager.Instance.IsBuildWatch)
            {
                uiRoot[i].worldCamera = Camera.main;
            }
            else
            {
                var clone = (GameObject)Instantiate(uiRoot[i].gameObject);
                clone.transform.SetParent(uiRoot[i].transform.parent);
                var cloneCanvas = clone.GetComponent<Canvas>();

                uiRoot[i].worldCamera = rightCamera;
                cloneCanvas.worldCamera = leftCamera;
                clone.name = uiRoot[i].name + "(Left)";
                uiRoot[i].name = uiRoot[i].name + "(Right)";
            }
        }
	}

    void Start() { }

}
