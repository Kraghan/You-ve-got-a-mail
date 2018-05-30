using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailboxCoordinator : MonoBehaviour
{
    [SerializeField]
    VacuumMailBox[] m_aVacuumMailboxes;
    GameObject m_player;

    uint m_activeMailbox = 0;

    [Header("UI")]
    [SerializeField]
    Transform m_follower;

    TargetFollower m_targetFollower;

    // Use this for initialization
    void Start ()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_targetFollower = gameObject.AddComponent<TargetFollower>();
        m_targetFollower.SetTarget(m_aVacuumMailboxes[0].transform);
        m_targetFollower.SetObjectToRotate(m_follower);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_aVacuumMailboxes[m_activeMailbox].IsDelivered())
        {
            if (m_activeMailbox < m_aVacuumMailboxes.Length)
            {
                m_activeMailbox++;
                m_aVacuumMailboxes[m_activeMailbox].SetAsCurrentTarget();
                m_targetFollower.SetTarget(m_aVacuumMailboxes[m_activeMailbox].transform);
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

    public void Reset()
    {
        m_activeMailbox = 0;
        for(uint i = 0; i < m_aVacuumMailboxes.Length; ++i)
        {
            m_aVacuumMailboxes[i].Reset();
        }
    }
}
