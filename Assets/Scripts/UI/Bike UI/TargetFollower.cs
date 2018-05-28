using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{

    [SerializeField]
    Transform m_target;

    Transform m_objectToRotate;

	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = m_target.position - m_objectToRotate.position;
        m_objectToRotate.rotation = Quaternion.LookRotation(direction);
	}

    public void SetTarget(Transform transf)
    {
        m_target = transf;
    }

    public void SetObjectToRotate(Transform transf)
    {
        m_objectToRotate = transf;
    }
}
