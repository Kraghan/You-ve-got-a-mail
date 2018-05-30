using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture_random : MonoBehaviour {

	public Material[] Couleurs;
	public Transform All_buildings;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void Color_them_all () {
		
		Transform[] Bat_all = All_buildings.GetComponentsInChildren<Transform> ();

		foreach (Transform lebat in Bat_all) {

			if (lebat.name == "Batiment") {

				Debug.Log(lebat.GetComponent<MeshRenderer>().materials);

				//lebat.GetComponent<MeshRenderer> ().material = lebat.GetComponent<Texture_random>().Couleurs[Random.Range (0, Couleurs.Length)];

			}		
		}
	}
}
