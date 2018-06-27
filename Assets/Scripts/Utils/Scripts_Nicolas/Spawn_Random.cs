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

    List<NavigationWaypoint> m_aPietonsSpot = new List<NavigationWaypoint>();
    List<NavigationWaypoint> m_aVoituressSpot = new List<NavigationWaypoint>();

    public List<NavigationWaypoint> Shuffle(List<NavigationWaypoint> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = (int)Random.value * n;
            NavigationWaypoint value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    void Start()
    {
        EntityPool = new GameObject("Entities").transform;
        PedestrianPool = new GameObject("Pedestrians").transform;
        CarPool = new GameObject("Cars").transform;

        PedestrianPool.parent = EntityPool;
        CarPool.parent = EntityPool;


        //Je récupères tous les scripts des noeuds des graphes
        All_nodes_pietons = Graphe_pieton.GetComponentsInChildren<NavigationWaypoint>();
        All_nodes_voitures = Graphe_voitures.GetComponentsInChildren<NavigationWaypoint>();

        //Le nombre de piétons et de voitures potentiellement spawnables
        int nbrob = 0;
        int nbcar = 0;

        //Je remplis les listes et les mélanges
        foreach (NavigationWaypoint waypoint in All_nodes_pietons) {
            m_aPietonsSpot.Add(waypoint);
        }

        Shuffle(m_aPietonsSpot);

        foreach (NavigationWaypoint waypoint in All_nodes_voitures) {
            m_aVoituressSpot.Add(waypoint);
        }
        Shuffle(m_aVoituressSpot);

        // Je place les entités sur les points
        foreach (NavigationWaypoint waypoint in All_nodes_pietons)
        {
            GameObject Pedestrian = CreateEntity(Pietons[Random.Range(0, Pietons.Length)], waypoint, 1, nbrob);

            Pedestrian.transform.parent = PedestrianPool;

            nbrob++;


            if (nbrob >= Max_pieton)
                break;
        }

        foreach (NavigationWaypoint waypoint in All_nodes_voitures)
        {
            GameObject Car = CreateEntity(Voitures[Random.Range(0, Voitures.Length)], waypoint, 4, nbcar);

            Car.transform.parent = CarPool;

            nbcar++;

            if (nbcar >= Max_voiture)
                break;
        }

        //J'affiche le nombre de piétons et de voitures potentiellement spawnables
        Debug.Log ("Robots " + nbrob);
		Debug.Log ("Voitures " + nbcar);

	}

    private GameObject CreateEntity(GameObject model, NavigationWaypoint startPoint, float speed, int id)
    {
        GameObject entity = Instantiate(model);
        entity.name += " (" + id + ")";
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
        GameObject pedestrian = CreateEntity(Pietons[Random.Range(0, Pietons.Length)], All_nodes_pietons[Random.Range(0,All_nodes_pietons.Length)], 1, -1);

        pedestrian.transform.parent = PedestrianPool;
    }
}
