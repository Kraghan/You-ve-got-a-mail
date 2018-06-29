using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailboxCoordinator : MonoBehaviour
{
    [SerializeField]
    public VacuumMailBox[] m_aVacuumMailboxes;

	//[HideInInspector]
    public uint m_activeMailbox = 0;

    [Header("UI")]
    [SerializeField]
    Transform m_follower;
    [SerializeField]
    Text m_distanceText;
	public Text Distance_Leisure;

    TargetFollower m_targetFollower;

	public Mode_selector The_mode;

    // Use this for initialization
    void Start ()
    {
        m_targetFollower = gameObject.AddComponent<TargetFollower>();
        m_targetFollower.SetTarget(m_aVacuumMailboxes[0].transform);
        m_targetFollower.SetObjectToRotate(m_follower);
        m_targetFollower.SetText(m_distanceText);
		m_targetFollower.SetText_Leisure(Distance_Leisure);
        //m_aVacuumMailboxes[0].SetAsCurrentTarget();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (The_mode.m_defaultPlayMode == Mode_selector.MyPlayMode.STORY) {
			if (m_aVacuumMailboxes [m_activeMailbox].IsDelivered ()) {
				if (m_activeMailbox < m_aVacuumMailboxes.Length - 1) {
					m_activeMailbox++;
					m_aVacuumMailboxes [m_activeMailbox].SetAsCurrentTarget ();
					m_targetFollower.SetTarget (m_aVacuumMailboxes [m_activeMailbox].transform);
				} else {
					Debug.Log ("End game");
				}
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
