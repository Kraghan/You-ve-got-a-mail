using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide_Buildings : MonoBehaviour {

	public GameObject[] Build_to_hide;

	void Update () {

		//Je check si le vélo est dans le collider, si c'est le cas, je cache les bâtiments de mon choix
		if (transform.position.z <= 230f) {

			foreach (GameObject Build in Build_to_hide)
				Build.SetActive (false);

		} else {
			
			foreach (GameObject Build in Build_to_hide)
				Build.SetActive (true);

		}
	}
}
