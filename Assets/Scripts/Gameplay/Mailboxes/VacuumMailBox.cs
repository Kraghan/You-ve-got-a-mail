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
    Transform m_snapPointStart;

    [SerializeField]
    Animator m_animator;

    [SerializeField]
    NavigationFollower m_follower;

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
        
        if(m_follower)
        {
            m_follower.enabled = false;
        }

        if(Vector3.Distance(m_snapPointStart.position,m_mail.transform.position) < 0.1)
        {
            Rigidbody body = m_mail.GetComponent<Rigidbody>();
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            m_mail.transform.rotation = m_snapPointStart.rotation;
            m_isDelivered = true;
            m_animator.SetBool("Open", false);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mail") && !m_mail && !m_isDelivered)
        {
            m_mail = other.gameObject;
            m_mail.GetComponent<Collider>().enabled = false;
            m_mail.GetComponent<Rigidbody>().useGravity = false;
			m_mail.GetComponent<Rigidbody>().velocity = Vector3.zero;

            m_animator.SetBool("Open", true);
        }
    }

    public void SetAsCurrentTarget()
    {
        m_isDelivered = false;
        if (m_mail != null)
            m_mail.SetActive(false);
    }

    public bool IsDelivered()
    {
        return m_isDelivered;
    }

    public void Reset()
    {
        m_isDelivered = false;
    }
}
