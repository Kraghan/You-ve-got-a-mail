using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour {

    [SerializeField]
    Transform m_target;
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = m_target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
	}

    public void SetTarget(Transform transf)
    {
        m_target = transf;
    }
}
