using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour {

    [SerializeField]
    MailboxCoordinator m_coordinator;
	
	// Update is called once per frame
	void Update ()
    {
        Transform target = m_coordinator.GetTarget();
        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
	}
}
