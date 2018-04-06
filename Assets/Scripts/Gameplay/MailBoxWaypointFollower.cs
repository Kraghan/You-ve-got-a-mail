using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBoxWaypointFollower : MonoBehaviour {

    private Transform[] m_transforms;
    private uint m_currentIndex = 0;

    [SerializeField]
    GameObject m_waypointContainer;
    [SerializeField]
    bool m_backtrackWhenOver = false;
    [SerializeField]
    bool m_loop = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDrawGizmosSelected()
    {
        if (!m_waypointContainer)
            return;

        MailBoxWaypoint[] children = m_waypointContainer.GetComponentsInChildren<MailBoxWaypoint>();
        Transform previous = null;
        for (uint index = 0; index < children.Length; ++index)
        {
            Transform child = children[index].transform;

            if(index == 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(child.position, 0.5f);
                
            }
            else if(index == children.Length - 1)
            {
                if (!m_backtrackWhenOver)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(child.position, 0.5f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previous.position, child.position);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previous.position, child.position);
                Gizmos.DrawWireSphere(child.position, 0.25f);
            }
            previous = child;
        }

    }

    public bool HasWaypointContainer()
    {
        return m_waypointContainer != null;
    }

    public void AddWaypoint()
    {
        int numberOfWaypoint = m_waypointContainer.GetComponentsInChildren<MailBoxWaypoint>().Length;

        GameObject newWaypoint = new GameObject(numberOfWaypoint.ToString());
        newWaypoint.AddComponent<MailBoxWaypoint>();
        newWaypoint.transform.position = transform.position;
        newWaypoint.transform.parent = m_waypointContainer.transform;
    }
}
