using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Spawn_Yawn : MonoBehaviour {

	public GameObject Robot_blue_left, Robot_Orange_left, Robot_blue_right, Robot_Orange_right;
	float proba;

	// Use this for initialization
	void Start () {

		proba = Random.Range (0, 100);

		if ((proba >= 50) && (proba <= 62))
			Robot_blue_left.SetActive (true);
		else if ((proba >= 63) && (proba <= 75))
			Robot_Orange_left.SetActive (true);
		else if ((proba >= 75) && (proba <= 87))
			Robot_blue_right.SetActive (true);
		else if ((proba >= 88) && (proba <= 100))
			Robot_Orange_right.SetActive (true);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
