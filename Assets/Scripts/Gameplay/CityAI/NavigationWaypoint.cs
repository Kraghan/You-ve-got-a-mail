using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NavigationType
{
    E_VEHICLE,
    E_ROBOT
}

public class NavigationWaypoint : MonoBehaviour
{
    [SerializeField]
    private NavigationType m_type;
    [SerializeField]
    private NavigationWaypoint[] m_neighbours;
    [SerializeField]
    private float m_distanceAutoDetectNeighbours = 5;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        
    }
}
