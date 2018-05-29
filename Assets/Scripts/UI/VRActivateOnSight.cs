using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRActivateOnSight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Disable();
	}

    public void Enable()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Disable()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
