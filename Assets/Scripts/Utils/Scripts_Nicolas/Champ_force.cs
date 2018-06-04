using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champ_force : MonoBehaviour {

	public GameObject Force_field;
    public GameObject Effet_circulation;

    private VacuumMailBox labox;

	// Use this for initialization
	void Start () {

        labox = GetComponentInChildren<VacuumMailBox>();

	}
	
	// Update is called once per frame
	void Update () {
		
        if (labox.IsDelivered())
        {
            
                Force_field.SetActive(false);
                Effet_circulation.SetActive(false);

        }

	}
}
