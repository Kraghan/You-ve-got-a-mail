using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootMode
{
    NONE,
    NATURAL,
    CANON
}

[RequireComponent(typeof(SteamVR_ControllerManager))]
public class ShootSelector : MonoBehaviour {

    GameObject m_controllerLeft = null;
    GameObject m_controllerRight = null;

    [SerializeField]
    ShootMode m_defaultMode = ShootMode.NATURAL;

    [SerializeField]
    ProbabilityOfAppearenceOfItem[] m_sendableObjects;

    [Header("Canon mode")]
    [SerializeField]
    float m_force = 20;

    [Header("Natural mode")]
    [SerializeField]
    float m_forceMultiplier = 2;

    Rigidbody m_bikeRigidbody;

    // Use this for initialization
    void Start ()
    {
        SteamVR_ControllerManager manager = GetComponent<SteamVR_ControllerManager>();
        m_controllerLeft = manager.left;
        m_controllerRight = manager.right;

        GameObject bike = GameObject.FindGameObjectWithTag("Player");
        m_bikeRigidbody = bike.GetComponent<Rigidbody>();
        SetModeTo(m_defaultMode);
    }
	
    public void SetModeTo(ShootMode mode)
    {
        // Remove all controllers
        MailCanon toRemoveCanon = m_controllerRight.GetComponent<MailCanon>();
        MailController toRemoveNatural = m_controllerRight.GetComponent<MailController>();

        if (toRemoveCanon != null)
            Destroy(toRemoveCanon);
        if (toRemoveNatural != null)
            Destroy(toRemoveNatural);

        toRemoveCanon = m_controllerLeft.GetComponent<MailCanon>();
        toRemoveNatural = m_controllerLeft.GetComponent<MailController>();

        if (toRemoveCanon != null)
            Destroy(toRemoveCanon);
        if (toRemoveNatural != null)
            Destroy(toRemoveNatural);

        if (mode == ShootMode.CANON)
        {
            MailCanon canon = m_controllerRight.AddComponent<MailCanon>();
            canon.SetForce(m_force);
            canon.SetBikeRigidbody(m_bikeRigidbody);
            canon.SetObjectToSend(m_sendableObjects);

            canon = m_controllerLeft.AddComponent<MailCanon>();
            canon.SetForce(m_force);
            canon.SetBikeRigidbody(m_bikeRigidbody);
            canon.SetObjectToSend(m_sendableObjects);
        }
        else if(mode == ShootMode.NATURAL)
        {
            MailController natural = m_controllerRight.AddComponent<MailController>();
            natural.SetMultiplier(m_forceMultiplier);
            natural.SetBikeRigidbody(m_bikeRigidbody);
            natural.SetObjectToSend(m_sendableObjects);

            natural = m_controllerLeft.AddComponent<MailController>();
            natural.SetMultiplier(m_forceMultiplier);
            natural.SetBikeRigidbody(m_bikeRigidbody);
            natural.SetObjectToSend(m_sendableObjects);
        }

    }
}
