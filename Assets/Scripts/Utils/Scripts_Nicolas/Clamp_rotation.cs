using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp_rotation : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

		float rotx = transform.localRotation.eulerAngles.x;
		rotx = rotx % 360f;

		if ((rotx < 40) || (rotx > 320f))
			return;
		else
			GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		
	}
}
