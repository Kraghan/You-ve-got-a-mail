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
    private ProbabilityOfAppearenceOfItem[] m_aMailsPrefabs;
    [SerializeField]
    private Rigidbody m_bikeRigibody;
    [SerializeField]
    private float m_forceMultiplier = 1.5f;

    Animator m_model;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        m_model = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update () {
        // Grab
        if (!m_model.GetBool("Grab") && !m_model.GetBool("Pointing") 
            && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger)
            && !m_newspaperInHand)
        {
            m_newspaperInHand = Instantiate(PickMail().gameObject);
            m_newspaperInHand.transform.position = transform.position;
            m_newspaperInHand.transform.rotation = transform.rotation;
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.breakForce = 20000;
            joint.breakTorque = 20000;
            joint.connectedBody = m_newspaperInHand.GetComponent<Rigidbody>();

            m_model.transform.parent.gameObject.SetActive(false);
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

                m_newspaperInHand.GetComponent<Rigidbody>().velocity = transform.parent.rotation * Quaternion.LookRotation(Controller.velocity) * Vector3.forward * Controller.velocity.magnitude * m_forceMultiplier + m_bikeRigibody.velocity;
                
                m_newspaperInHand.GetComponent<Rigidbody>().angularVelocity = (Controller.angularVelocity * m_forceMultiplier) + m_bikeRigibody.angularVelocity;
            }
            m_newspaperInHand = null;
            m_model.transform.parent.gameObject.SetActive(true);
        }
    }

    public void SetMultiplier(float multiplier)
    {
        m_forceMultiplier = multiplier;
    }

    public void SetBikeRigidbody(Rigidbody body)
    {
        m_bikeRigibody = body;
    }

    private Mail PickMail()
    {

        float maxPercentage = 0;
        foreach(ProbabilityOfAppearenceOfItem proba in m_aMailsPrefabs)
        {
            maxPercentage += proba.m_probability;
        }

        float value = Random.Range(0, maxPercentage);
        float storage = 0;
        foreach (ProbabilityOfAppearenceOfItem proba in m_aMailsPrefabs)
        {
            storage += proba.m_probability;
            if (value < storage)
                return proba.m_item;
        }

        return null;
    }

    public void SetObjectToSend(ProbabilityOfAppearenceOfItem[] items)
    {
        m_aMailsPrefabs = items;
    }
}