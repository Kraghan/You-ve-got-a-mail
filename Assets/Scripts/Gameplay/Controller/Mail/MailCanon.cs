using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MailCanon : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    [SerializeField]
    private ProbabilityOfAppearenceOfItem[] m_aMailsPrefabs;
    [SerializeField]
    private float m_force = 20;
    [SerializeField]
    private Rigidbody m_bikeBody;
    Animator m_model;

    private bool m_throwNewspaper = false;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        m_model = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!m_model.GetBool("Grab") && !m_model.GetBool("Pointing") 
            && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            m_throwNewspaper = true;
            m_model.SetBool("Gun", true);
        }
        else if (m_throwNewspaper && !Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            GetComponent<LineRenderer>().enabled = false;
            ThrowNewspaper();
            m_throwNewspaper = false;
            m_model.SetBool("Gun", false);
        }
    }
    
    void ThrowNewspaper()
    {
        Vector3 direction = transform.forward;
        direction.Normalize();

        GameObject newspaper = Instantiate(PickMail().gameObject, transform.position, transform.rotation) as GameObject;
        Rigidbody body = newspaper.GetComponent<Rigidbody>();
        body.angularVelocity = m_bikeBody.angularVelocity;
        body.velocity = m_bikeBody.velocity;
        body.AddForce(direction * m_force, ForceMode.Impulse);
        AkSoundEngine.PostEvent("YGM_Gunshot", gameObject);
    }

    public void SetForce(float force)
    {
        m_force = force;
    }

    public void SetObjectToSend(ProbabilityOfAppearenceOfItem[] items)
    {
        m_aMailsPrefabs = items;
    }

    public void SetBikeRigidbody(Rigidbody body)
    {
        m_bikeBody = body;
    }

    private Mail PickMail()
    {

        float maxPercentage = 0;
        foreach (ProbabilityOfAppearenceOfItem proba in m_aMailsPrefabs)
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
}
