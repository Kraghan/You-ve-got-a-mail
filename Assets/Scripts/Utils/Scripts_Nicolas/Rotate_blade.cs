using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_blade : MonoBehaviour {

	public Transform Center;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Je fais une rotation à la bonne vitesse autour du centre de l'hélice
		transform.RotateAround(Center.transform.position, Center.transform.forward, speed * Time.deltaTime);
		
	}
}
