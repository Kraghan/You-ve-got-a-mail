using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseShootSelector : MonoBehaviour {

    
	GameObject m_controller = null;

    [SerializeField]
    ShootMode m_defaultMode = ShootMode.NATURAL;

    [SerializeField]
    ProbabilityOfAppearenceOfItem[] m_sendableObjects;

    [Header("Canon mode")]
    [SerializeField]
    float m_minForce = 2;
    [SerializeField]
    float m_maxForce = 12;
    [SerializeField]
    Timer m_timeToReachMaxForce;


    [Header("Natural mode")]
    [SerializeField]
    float m_forceMultiplier = 2;

    Rigidbody m_bikeRigidbody;

	public float precision = 0.1f;
	public int nbpoints = 50;

    // Use this for initialization
    void Start ()
    {
		m_controller = this.gameObject;

        GameObject bike = GameObject.FindGameObjectWithTag("Player");
        m_bikeRigidbody = bike.GetComponent<Rigidbody>();
        SetModeTo(m_defaultMode);

    }
	
    public void SetMode(bool canon)
    {
        if (canon)
            SetModeTo(ShootMode.CANON);
        else
            SetModeTo(ShootMode.NATURAL);

    }

    public void SetModeTo(ShootMode mode)
    {
        // Remove all controllers
		MouseMailCanon toRemoveCanon = m_controller.GetComponent<MouseMailCanon>();
        MailController toRemoveNatural = m_controller.GetComponent<MailController>();

        if (toRemoveCanon != null)
            Destroy(toRemoveCanon);
        if (toRemoveNatural != null)
            Destroy(toRemoveNatural);

        if (mode == ShootMode.CANON)
        {
			MouseMailCanon canon = m_controller.AddComponent<MouseMailCanon>();
            canon.SetForce(m_minForce,m_maxForce);
            canon.SetBikeRigidbody(m_bikeRigidbody);
            canon.SetObjectToSend(m_sendableObjects);
            canon.SetLaserOrigin(m_controller.transform.Find("LaserStart"));
			canon.SetPrecision (precision);
			canon.SetPoints (nbpoints);

            canon.SetAnimator();
            canon.SetTimer(m_timeToReachMaxForce);
        }
        else if(mode == ShootMode.NATURAL)
        {
            MailController natural = m_controller.AddComponent<MailController>();
            natural.SetMultiplier(m_forceMultiplier);
            natural.SetBikeRigidbody(m_bikeRigidbody);
            natural.SetObjectToSend(m_sendableObjects);
            
            natural.SetAnimator();
        }

    }
}
