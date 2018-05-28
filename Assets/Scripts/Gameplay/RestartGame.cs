using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [SerializeField]
    SafePylone m_firstPylone;

    GameObject m_player;

    CrashDetection m_detection;

    [SerializeField]
    MailboxCoordinator m_coordinator;

	// Use this for initialization
	void Start ()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_detection = m_player.GetComponent<CrashDetection>();
        m_detection.SetRespawnPylone(m_firstPylone);
        m_coordinator.Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
