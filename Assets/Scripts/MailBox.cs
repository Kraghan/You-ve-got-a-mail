using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour {

    private Material m_debugMaterial;
    private GameObject m_letterIn;
    private Vector3 m_oldPosition;
    private Timer m_timer;

	// Use this for initialization
	void Start ()
    {
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        m_debugMaterial = renderer.material;
    }

    void Update()
    {
        if (!m_letterIn)
            return;

        if (Vector3.Distance(m_oldPosition, m_letterIn.transform.position) < 0.5f)
            m_timer.UpdateTimer();
        else
        {
            m_timer.Restart();
            m_oldPosition = m_letterIn.transform.position;
        }

        if(m_timer.IsTimedOut())
        {
            m_debugMaterial.color = Color.green;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mail"))
        {
            if(!m_letterIn)
                m_letterIn = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mail"))
        {
            if(other.gameObject.GetInstanceID() == m_letterIn.GetInstanceID())
                m_letterIn = null;

        }
    }
}
