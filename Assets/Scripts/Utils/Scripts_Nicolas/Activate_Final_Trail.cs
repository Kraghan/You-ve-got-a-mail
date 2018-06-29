using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Final_Trail : MonoBehaviour {

	public MailboxCoordinator The_Coordinator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (The_Coordinator.m_activeMailbox == 10)
			this.GetComponent<Effet_energie> ().enabled = true;
		else
			this.GetComponent<Effet_energie> ().enabled = false;

	}
}
