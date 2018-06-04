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

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, m_distanceAutoDetectNeighbours);*/
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.75f);

        for(int i = 0; i < m_neighbours.Length; ++i)
        {
            if(m_neighbours[i] != null)
            {
                Vector3 pos = transform.position;
                Vector3 direction = m_neighbours[i].transform.position - transform.position;
                Color color = Color.green;
                float arrowHeadLength = 0.25f;
                float arrowHeadAngle = 20;

                Gizmos.color = color;
                Gizmos.DrawRay(pos, direction);
                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            }
        }
    }

    public NavigationWaypoint GetRandomNeighbour()
    {
        if (m_neighbours.Length == 0)
            return null;

        int rnd = Random.Range(0, m_neighbours.Length);
        if(m_neighbours[rnd] == null)
        {
            do
            {
                rnd--;
            } while (rnd >= 0 && m_neighbours[rnd] == null);
        }

        if (rnd >= 0)
            return m_neighbours[rnd];
        else
            return null;
    }
}
