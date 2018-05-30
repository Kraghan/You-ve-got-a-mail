using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Random : MonoBehaviour {

	public GameObject Graphe_pieton;
	public GameObject Graphe_voitures;
	public GameObject Pieton;
	public GameObject Voiture;

    public uint Max_voiture = 30;
    public uint Max_pieton = 50;

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
			if (Random.value <= 0.5f) {
				GameObject lepieton = Instantiate (Pieton);
				lepieton.GetComponent<NavigationFollower> ().SetStartPoint(waypoint);
				//lepieton.GetComponent<NavigationFollower> ().SetSpeed(Random.Range (1, 2) + Random.value);
				lepieton.GetComponent<NavigationFollower> ().SetSpeed(1);
				nbrob++;
			}

            if (nbrob >= Max_pieton)
                break;
		}

		//Je fais popper les voitures sur tous les noeuds du graphe des voitures
		foreach (NavigationWaypoint waypoint in All_nodes_voitures) {
			if (Random.value <= 0.75f) {
				GameObject lavoiture = Instantiate (Voiture);
				lavoiture.GetComponent<NavigationFollower> ().SetStartPoint(waypoint);
				//lavoiture.GetComponent<NavigationFollower> ().SetSpeed(Random.Range (3, 4) + Random.value);
				lavoiture.GetComponent<NavigationFollower> ().SetSpeed(4);
				nbcar++;
			}

            if (nbcar >= Max_voiture)
                break;
        }

		//J'affiche le nombre de piétons et de voitures potentiellement spawnables
		Debug.Log ("Robots " + nbrob);
		Debug.Log ("Voitures " + nbcar);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
