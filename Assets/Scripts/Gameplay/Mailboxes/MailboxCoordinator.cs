using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxCoordinator : MonoBehaviour
{
    [SerializeField]
    VacuumMailBox[] m_aVacuumMailboxes;
    GameObject m_player;

    uint m_activeMailbox = 0;

	// Use this for initialization
	void Start ()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(m_aVacuumMailboxes[m_activeMailbox].IsDelivered())
        {
            if (m_activeMailbox < m_aVacuumMailboxes.Length)
            {

                m_activeMailbox++;
                m_aVacuumMailboxes[m_activeMailbox].SetAsCurrentTarget();
            }
            else
            {
                // End game
            }
        }
	}

    public uint GetMailboxActive()
    {
        return m_activeMailbox;
    }

    public Transform GetTarget()
    {
        return m_aVacuumMailboxes[m_activeMailbox].transform;
    }
}
