using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate_Timer : MonoBehaviour {

	public GameObject Object;

	public float TheTime;

	private float timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer >= TheTime)
			Object.SetActive (false);

	}
}
