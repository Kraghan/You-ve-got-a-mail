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

    [SerializeField]
    Material m_deliveredMaterial;
    [SerializeField]
    Material m_targetMaterial;
    [SerializeField]
    Material m_normalMaterial;

    [SerializeField]
    float m_blinkTimeRatio;
    [SerializeField]
    Color m_baseColorEmissive = new Color(0, 1, 1);
    [SerializeField]
    ParticleSystem m_particles;
    [SerializeField]
    Timer m_timeMailScale;

    bool m_isTarget = false;

    [SerializeField]
    bool m_infiniteMailbox = false;

	// Use this for initialization
	void Start ()
    {
        m_rotationSpeedAxis.x = Random.Range(0, m_maxRotationSpeedAxis.x);
        m_rotationSpeedAxis.y = Random.Range(0, m_maxRotationSpeedAxis.y);
        m_rotationSpeedAxis.z = Random.Range(0, m_maxRotationSpeedAxis.z);
        m_particles.Stop();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_infiniteMailbox && m_isDelivered)
            m_isDelivered = false;

        if (m_isTarget)
        {
            SetMaterialEmissive();
        }

        if (m_mail == null)
            return;
        
        if(m_follower)
        {
            m_follower.enabled = false;
        }

        if(m_isDelivered && m_mail)
        {
            m_timeMailScale.UpdateTimer();
            float scale = Mathf.Lerp(1,0,m_timeMailScale.GetRatio());
            m_mail.transform.localScale = new Vector3(scale,scale,scale);

            if(m_timeMailScale.IsTimedOut())
            {
                Destroy(m_mail);
                m_mail = null;
            }
        }

        if (m_isDelivered)
            return;

        if(Vector3.Distance(m_snapPointStart.position,m_mail.transform.position) < 0.1)
        {
            Rigidbody body = m_mail.GetComponent<Rigidbody>();
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            m_mail.transform.rotation = m_snapPointStart.rotation;
            m_isDelivered = true;
            m_animator.SetBool("Open", false);

            SetMaterial(m_deliveredMaterial);
            m_isTarget = false;
            m_particles.Play();

            m_timeMailScale.Restart();
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
        {
            Destroy(m_mail);
            m_mail = null;
        }
        SetMaterial(m_targetMaterial);
        m_isTarget = true;

    }

    public bool IsDelivered()
    {
        return m_isDelivered;
    }

    public void Reset()
    {
        m_isDelivered = false;
        
    }

    void SetMaterial(Material mat)
    {
        MeshRenderer[] renderers = m_animator.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = mat;
            renderer.material.SetColor("_EmissionColor", m_baseColorEmissive);
        }
    }

    void SetMaterialEmissive()
    {
        MeshRenderer[] renderers = m_animator.GetComponentsInChildren<MeshRenderer>();
        float emission = Mathf.PingPong(Time.time * m_blinkTimeRatio, 1.0f);

        Color finalColor = m_baseColorEmissive * Mathf.LinearToGammaSpace(emission);

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.SetColor("_EmissionColor", finalColor);
        }
    }
}
