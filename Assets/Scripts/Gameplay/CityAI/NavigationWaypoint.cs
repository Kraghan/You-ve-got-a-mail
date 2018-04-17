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
    /*[SerializeField]
    private float m_distanceAutoDetectNeighbours = 5;*/



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, m_distanceAutoDetectNeighbours);*/
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);

        for(int i = 0; i < m_neighbours.Length; ++i)
        {
            if(m_neighbours[i] != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, m_neighbours[i].transform.position);
            }
        }
    }
}
