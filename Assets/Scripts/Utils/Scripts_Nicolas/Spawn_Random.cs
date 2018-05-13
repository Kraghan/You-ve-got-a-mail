﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Random : MonoBehaviour {

	public GameObject Graphe_pieton;
	public GameObject Graphe_voitures;
	public GameObject Pieton;
	public GameObject Voiture;

	// Use this for initialization
	void Start () {

		//Je récupères tous les scripts des noeuds des graphes
		NavigationWaypoint[] All_nodes_pietons = Graphe_pieton.GetComponentsInChildren<NavigationWaypoint> ();
		NavigationWaypoint[] All_nodes_voitures = Graphe_voitures.GetComponentsInChildren<NavigationWaypoint> ();

		//Le nombre de piétons et de voitures potentiellement spawnables
		int nbrob = 0;
		int nbcar = 0;

		//Je fais popper les piétons sur tous les noeuds du graphe piéton
		foreach (NavigationWaypoint waypoint in All_nodes_pietons) {
			if (Random.value >= 0.3f) {
				GameObject lepieton = Instantiate (Pieton);
				lepieton.GetComponent<NavigationFollower> ().m_startPoint = waypoint;
				lepieton.GetComponent<NavigationFollower> ().m_speed = Random.Range (1, 2) + Random.value;
				nbrob++;
			}
		}

		//Je fais popper les voitures sur tous les noeuds du graphe des voitures
		foreach (NavigationWaypoint waypoint in All_nodes_voitures) {
			if (Random.value >= 0.1f) {
				GameObject lavoiture = Instantiate (Voiture);
				lavoiture.GetComponent<NavigationFollower> ().m_startPoint = waypoint;
				lavoiture.GetComponent<NavigationFollower> ().m_speed = Random.Range (4, 5) + Random.value;
				nbcar++;
			}
		}

		//J'affiche le nombre de piétons et de voitures potentiellement spawnables
		Debug.Log ("Robots " + nbrob);
		Debug.Log ("Voitures " + nbcar);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
