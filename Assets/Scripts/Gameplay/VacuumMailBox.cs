using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumMailBox : MonoBehaviour {

    bool m_isDelivered = false;
    GameObject m_mail = null;

    [SerializeField]
    float m_vacuumSpeed = 1;
    [SerializeField]
    Vector3 m_maxRotationSpeedAxis;
    Vector3 m_rotationSpeedAxis;

    [SerializeField]
    Timer m_timeToAnimate;

    [SerializeField]
    Transform m_snapPointStart;
    [SerializeField]
    Transform m_snapPointEnd;

    [SerializeField]
    MeshRenderer m_rendererDebug;

	// Use this for initialization
	void Start ()
    {
        m_rotationSpeedAxis.x = Random.Range(0, m_maxRotationSpeedAxis.x);
        m_rotationSpeedAxis.y = Random.Range(0, m_maxRotationSpeedAxis.y);
        m_rotationSpeedAxis.z = Random.Range(0, m_maxRotationSpeedAxis.z);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_mail == null)
            return;

        if(!m_isDelivered)
        {
            if(Vector3.Distance(m_snapPointStart.position,m_mail.transform.position) < 0.1)
            {
                Rigidbody body = m_mail.GetComponent<Rigidbody>();
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                m_mail.transform.rotation = m_snapPointStart.rotation;
                m_isDelivered = true;
            }
            else
            {
                Vector3 mailDirection = m_snapPointStart.position - m_mail.transform.position;
                mailDirection.Normalize();
                Rigidbody body = m_mail.GetComponent<Rigidbody>();
                body.velocity = new Vector3(body.velocity.x, body.velocity.y, body.velocity.z);

                m_mail.transform.position += mailDirection * m_vacuumSpeed * Time.deltaTime;
                m_mail.transform.Rotate(m_rotationSpeedAxis * Time.deltaTime);
            }
            
        }

		if(m_isDelivered)
        {
            m_rendererDebug.material.color = Color.green;
            m_timeToAnimate.UpdateTimer();
            m_mail.transform.position = Vector3.Lerp(m_snapPointStart.position, m_snapPointEnd.position, m_timeToAnimate.GetRatio());
            if (m_timeToAnimate.IsTimedOut())
            {
                Destroy(m_mail);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mail"))
        {
            m_mail = other.gameObject;
            m_mail.GetComponent<Collider>().enabled = false;
            m_mail.GetComponent<Rigidbody>().useGravity = false;
            m_timeToAnimate.Start();
        }
    }
}
