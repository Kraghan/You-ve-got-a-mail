using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_on_delivery : MonoBehaviour {

	public BoxCollider exploder;

	// Update is called once per frame
	void OnTriggerEnter (Collider other) {

		if (other.tag == "Mail")
			exploder.enabled = true;
		
	}
}
