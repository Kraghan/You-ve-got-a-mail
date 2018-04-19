using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private GameObject m_newspaperInHand;

    [SerializeField]
    private GameObject m_newspaperPool;

    [SerializeField]
    private GameObject m_newspaperPrefab;
    [SerializeField]
    private Rigidbody m_bikeRigibody;
    [SerializeField]
    private float m_forceMultiplier = 1.5f;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
        // Grab
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)
            && !m_newspaperInHand)
        {
            m_newspaperInHand = Instantiate(m_newspaperPrefab);
            m_newspaperInHand.transform.position = transform.position;
            m_newspaperInHand.transform.rotation = transform.rotation;
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.breakForce = 20000;
            joint.breakTorque = 20000;
            joint.connectedBody = m_newspaperInHand.GetComponent<Rigidbody>();
        }
        // Release
        else if (!Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)
            && m_newspaperInHand)
        {
            FixedJoint joint = gameObject.GetComponent<FixedJoint>();
            if (joint)
            {
                joint.connectedBody = null;
                Destroy(joint);

                m_newspaperInHand.GetComponent<Rigidbody>().velocity = (Controller.velocity * m_forceMultiplier) + m_bikeRigibody.velocity;
                m_newspaperInHand.GetComponent<Rigidbody>().angularVelocity = (Controller.angularVelocity * m_forceMultiplier) + m_bikeRigibody.angularVelocity;
            }
            m_newspaperInHand.transform.parent = m_newspaperPool.transform;
            m_newspaperInHand = null;
        }
    }
}

/*
 * + Sympathique
 * + Réfléchi
 * + Pédagogue
 * 
 * - Ecoute peu
 * - Trop confiant
 * - Difficulté à me faire comprendre
 */