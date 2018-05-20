using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxCoordinator : MonoBehaviour
{
    [SerializeField]
    VacuumMailBox[] m_aVacuumMailboxes;

    TargetFollower m_playerUITarget;
    GameObject m_player;

    uint m_activeMailbox = 0;

	// Use this for initialization
	void Start ()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_playerUITarget = m_player.GetComponentInChildren<TargetFollower>();
        if (m_aVacuumMailboxes.Length != 0)
            m_playerUITarget.SetTarget(m_aVacuumMailboxes[0].transform);
        else
            Debug.LogError("No mailboxes set in Mailbox coordinator");
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
                m_playerUITarget.SetTarget(m_aVacuumMailboxes[m_activeMailbox].transform);
            }
            else
            {
                // End game
            }
        }
	}
}
