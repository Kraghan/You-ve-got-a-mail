using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effet_energie : MonoBehaviour {

	public Transform parentsalves;
	public Transform line_effect;
	public float frequency;
	public float speed;

	public NavigationWaypoint startpoint;
	public NavigationWaypoint endpoint;

	private float temps;
	private Transform lasalve;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Je calcule le temps qui passe d'ici à la prochaine salve
		if (temps <= frequency) {
			
			temps += Time.deltaTime;

		//Si assez de temps est passé, je génère une salve
		} else {
			
			lasalve = Instantiate (line_effect, startpoint.transform.position, Quaternion.identity);
			lasalve.parent = parentsalves;
			lasalve.GetComponent<NavigationFollower> ().SetStartPoint (startpoint);
			lasalve.GetComponent<NavigationFollower> ().SetSpeed (speed);
			temps = 0;

		}
			
		foreach (NavigationFollower salve in parentsalves.GetComponentsInChildren<NavigationFollower>()) {

			//Si cette salve est arrivée au bout du chemin, je la détruit
			if (salve.m_nextTarget == endpoint)
				salve.enabled = false;

		}		
	}
}
