using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture_random : MonoBehaviour {

	public Material[] Couleurs;
	public Transform All_buildings;

	// Use this for initialization
	void Start () {
		
	}
	
	//Fonction de couleurs aléatoires dans notre palette
	public void Color_them_all () {
		
		Transform[] Bat_all = All_buildings.GetComponentsInChildren<Transform> ();

		foreach (Transform lebat in Bat_all) {

			if (lebat.name == "Batiment") {

				//Debug.Log("Ancien" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

				Material[] mesmats = lebat.GetComponent<Renderer> ().sharedMaterials;

				mesmats[0] = Couleurs[Random.Range(0, Couleurs.Length)];

				lebat.GetComponent<Renderer> ().sharedMaterials = mesmats;

				//Debug.Log("Nouveau" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

			}		
		}
	}

	//Fonction saruman du fun
	public void Saruman_the_multicolored () {

		Transform[] Bat_all = All_buildings.GetComponentsInChildren<Transform> ();

		foreach (Transform lebat in Bat_all) {

			if (lebat.name == "Batiment") {

				//Debug.Log("Ancien" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

				Material[] mesmats = lebat.GetComponent<Renderer> ().materials;

				mesmats[0].color = Random.ColorHSV();

				lebat.GetComponent<Renderer> ().materials = mesmats;

				//Debug.Log("Nouveau" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

			}		
		}
	}
}
