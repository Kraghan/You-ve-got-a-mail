using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Random : MonoBehaviour {

	public GameObject Graphe_pieton;
	public GameObject Graphe_voitures;
	public GameObject[] Pietons;
	public GameObject[] Voitures;

    public uint Max_voiture = 30;
    public uint Max_pieton = 50;

    private NavigationWaypoint[] All_nodes_pietons;
    private NavigationWaypoint[] All_nodes_voitures;

    private Transform EntityPool;
    private Transform PedestrianPool;
    private Transform CarPool;

    void Start ()
    {
        EntityPool = new GameObject("Entities").transform;
        PedestrianPool = new GameObject("Pedestrians").transform;
        CarPool = new GameObject("Cars").transform;

        PedestrianPool.parent = EntityPool;
        CarPool.parent = EntityPool;


        //Je récupères tous les scripts des noeuds des graphes
        All_nodes_pietons = Graphe_pieton.GetComponentsInChildren<NavigationWaypoint> ();
		All_nodes_voitures = Graphe_voitures.GetComponentsInChildren<NavigationWaypoint> ();

		//Le nombre de piétons et de voitures potentiellement spawnables
		int nbrob = 0;
		int nbcar = 0;

		//Je fais popper les piétons sur tous les noeuds du graphe piéton
		foreach (NavigationWaypoint waypoint in All_nodes_pietons) {
			if (Random.value <= 0.5f)
            {
                GameObject Pedestrian = CreateEntity(Pietons[Random.Range(0, Pietons.Length)], waypoint, 1);

                Pedestrian.transform.parent = PedestrianPool;

				nbrob++;
			}

            if (nbrob >= Max_pieton)
                break;
		}

		//Je fais popper les voitures sur tous les noeuds du graphe des voitures
		foreach (NavigationWaypoint waypoint in All_nodes_voitures) {
			if (Random.value <= 0.75f) {
                GameObject Car = CreateEntity(Voitures[Random.Range(0, Pietons.Length)], waypoint, 4);

                Car.transform.parent = CarPool;

				nbcar++;
			}

            if (nbcar >= Max_voiture)
                break;
        }

		//J'affiche le nombre de piétons et de voitures potentiellement spawnables
		Debug.Log ("Robots " + nbrob);
		Debug.Log ("Voitures " + nbcar);

	}

    private GameObject CreateEntity(GameObject model, NavigationWaypoint startPoint, float speed)
    {
        GameObject entity = Instantiate(model);
        entity.GetComponent<NavigationFollower>().SetStartPoint(startPoint);
        entity.GetComponent<NavigationFollower>().SetSpeed(speed);
        RagdollTriggerer ragdoll = entity.GetComponentInChildren<RagdollTriggerer>();
        if(ragdoll)
        {
            ragdoll.SetSpawner(this);
        }

        return entity;
    }

    public void CreatePedestrian()
    {
        GameObject pedestrian = CreateEntity(Pietons[Random.Range(0, Pietons.Length)], All_nodes_pietons[Random.Range(0,All_nodes_pietons.Length)], 1);

        pedestrian.transform.parent = PedestrianPool;
    }
}
