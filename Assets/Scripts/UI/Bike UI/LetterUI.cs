using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MailIcon
{
    public GameObject m_iconOff;
    public GameObject m_iconOn;
}


public class LetterUI : MonoBehaviour
{
    [SerializeField]
    MailboxCoordinator m_coordinator;

    [SerializeField]
    MailIcon[] m_icons;

	// Use this for initialization
	void Start ()
    {
        for (uint i = 0; i < m_icons.Length; ++i)
        {
            m_icons[i].m_iconOff.SetActive(true);
            m_icons[i].m_iconOn.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        for (uint i = 0; i < m_coordinator.GetMailboxActive(); ++i)
        {
            m_icons[i].m_iconOff.SetActive(false);
            m_icons[i].m_iconOn.SetActive(true);
        }
    }
}
