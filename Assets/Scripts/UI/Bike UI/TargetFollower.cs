using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetFollower : MonoBehaviour
{

    [SerializeField]
    Transform m_target;
    [SerializeField]
    Text m_text;

    Transform m_objectToRotate;

	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = m_target.position - m_objectToRotate.position;
        m_objectToRotate.rotation = Quaternion.LookRotation(direction);

        if(m_text)
            m_text.text = Mathf.Round(Vector3.Distance(m_target.position, m_objectToRotate.position)) + " m";
	}

    public void SetTarget(Transform transf)
    {
        m_target = transf;
    }

    public void SetObjectToRotate(Transform transf)
    {
        m_objectToRotate = transf;
    }

    public void SetText(Text text)
    {
        m_text = text;
    }
}
