using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian_Eraser : MonoBehaviour {

	private bool canhide;
	public float zpos = 230f;
	public bool north = true;
	public Transform Bikey;

	List<GameObject> Hidden_Pedestrians = new List<GameObject>();
	private bool doonce;

	void Update () {

		if (north) {
			//Je check si le vélo est à la position que je veux, si c'est le cas, je cache les piétons de mon choix
			if (Bikey.position.z <= zpos) {
				doonce = false;
				canhide = true;
			} else {
				if (!doonce) {
					//J'affiche tous les piétons cachés de la zone concernée
					for(int i = 0; i < Hidden_Pedestrians.Count; ++i) {
						ShowThePedestrian (Hidden_Pedestrians [i]);
					}

					doonce = true;
				}
				canhide = false;
			}
		}

		else if (!north) {
			//Je check si le vélo est à la position que je veux, si c'est le cas, je cache les piétons de mon choix
			if (Bikey.position.z >= zpos) {
				doonce = false;
				canhide = true;
			} else {
				
				if (!doonce) {
					//J'affiche tous les piétons cachés de la zone concernée
					for(int i = 0; i < Hidden_Pedestrians.Count; ++i) {
						ShowThePedestrian (Hidden_Pedestrians [i]);
					}

					doonce = true;
				}
				canhide = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (canhide) {
			if ((other.CompareTag ("Mover")) && (other.GetComponent<Can_deactivate> () != null)) {
				Hidden_Pedestrians.Add (other.gameObject);
				HideThePedestrian (other.gameObject);
			} else if ((other.CompareTag ("Mover")) && (other.GetComponentInParent<Can_deactivate> () != null)) {
				Hidden_Pedestrians.Add (other.GetComponentInParent<Can_deactivate> ().gameObject);
				HideThePedestrian (other.GetComponentInParent<Can_deactivate> ().gameObject);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (canhide) {
			if ((other.CompareTag ("Mover")) && (other.GetComponent<Can_deactivate> () != null)) {
				Hidden_Pedestrians.Remove (other.gameObject);
				ShowThePedestrian (other.gameObject);
			} else if ((other.CompareTag ("Mover")) && (other.GetComponentInParent<Can_deactivate> () != null)) {
				Hidden_Pedestrians.Remove (other.GetComponentInParent<Can_deactivate> ().gameObject);
				ShowThePedestrian (other.GetComponentInParent<Can_deactivate> ().gameObject);
			}
		}
	}

	void HideThePedestrian (GameObject other) {
		
		MeshRenderer[] lesmeshes = other.GetComponentsInChildren<MeshRenderer> ();

		foreach (MeshRenderer lemesh in lesmeshes) {
			lemesh.enabled = false;		
		}

		SkinnedMeshRenderer[] skinmeshes = other.GetComponentsInChildren<SkinnedMeshRenderer> ();

		foreach (SkinnedMeshRenderer leskin in skinmeshes) {
			leskin.enabled = false;		
		}

	}

	void ShowThePedestrian (GameObject other) {

		MeshRenderer[] lesmeshes = other.GetComponentsInChildren<MeshRenderer> ();

		foreach (MeshRenderer lemesh in lesmeshes) {
			lemesh.enabled = true;	
		}

		SkinnedMeshRenderer[] skinmeshes = other.GetComponentsInChildren<SkinnedMeshRenderer> ();

		foreach (SkinnedMeshRenderer leskin in skinmeshes) {
			leskin.enabled = true;	
		}
	}
}