using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dont_Show : MonoBehaviour {

	public GameObject Build_to_hide;

	void Update () {

		//Je check si le vélo est dans le collider, si c'est le cas, je cache les bâtiments de mon choix
		if (transform.position.z <= 230f) {

			MeshRenderer[] lesmeshs = Build_to_hide.GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer lemesh in lesmeshs) {

				lemesh.enabled = false;

			}

		} else {

			MeshRenderer[] lesmeshs = Build_to_hide.GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer lemesh in lesmeshs) {

				lemesh.enabled = true;

			}
		}
	}
}