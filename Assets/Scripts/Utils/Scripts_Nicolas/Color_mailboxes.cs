using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_mailboxes : MonoBehaviour {
	
	public Material Couleur;
	public Transform All_boxes;

	//Fonction de couleurs aléatoires dans notre palette
	public void Color_all_mailboxes () {

		Transform[] Boxes_all = All_boxes.GetComponentsInChildren<Transform> ();

		foreach (Transform labox in Boxes_all) {
			
			if (labox.GetComponent<MeshRenderer> () != null) {

				//Je récupère son matériau
				Material monmat = labox.GetComponent<MeshRenderer> ().sharedMaterial;

				monmat = Couleur;

				//Je donne cette couleur au mur pour de bon
				labox.GetComponent<MeshRenderer> ().sharedMaterial = monmat;

			}
		}
	}
}
