using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champ_force : MonoBehaviour {

	public GameObject Force_field;
    public GameObject Effet_circulation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Quand un objet rentre en contact avec le levier
	void OnCollisionEnter(Collision other)
	{

		//Si cet objet est un courrier
		if (other.collider.tag == "Mail") {

			Force_field.SetActive (false);
            Effet_circulation.SetActive(false);

        }
	}
}
