using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian_Eraser : MonoBehaviour {

	List<GameObject> m_PedestriansInSpawnArea = new List<GameObject>();

	void Update () {

		//Je check si le vélo est à la position que je veux, si c'est le cas, je cache les bâtiments de mon choix
		if (transform.position.z <= 230f) {

			HideThePedestrians ();

		} else {

			ShowThePedestrians ();

		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Mover"))
			m_PedestriansInSpawnArea.Add (other.GetComponentInParent<Transform>().gameObject);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Mover"))
			m_PedestriansInSpawnArea.Remove (other.GetComponentInParent<Transform>().gameObject);
	}

	void HideThePedestrians () {

		for(int i = 0; i < m_PedestriansInSpawnArea.Count; ++i)
		{

			MeshRenderer[] lesmeshes = m_PedestriansInSpawnArea[i].GetComponentsInChildren<MeshRenderer> ();

			foreach (MeshRenderer lemesh in lesmeshes) {
				lemesh.enabled = false;
			}
		}

	}

	void ShowThePedestrians () {

		for(int i = 0; i < m_PedestriansInSpawnArea.Count; ++i)
		{
			MeshRenderer[] lesmeshes =  m_PedestriansInSpawnArea[i].GetComponentsInChildren<MeshRenderer> ();

			foreach (MeshRenderer lemesh in lesmeshes) {
				lemesh.enabled = true;
			}
		}

	}
}