using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollTriggerer : MonoBehaviour {

    Rigidbody[] m_aBodies;
    Animator m_animator;
    NavigationFollower m_follower;

    [SerializeField]
    [Tooltip("The base weight for robots is 60 kg, the factor is the divide factor to modify this weight")]
    float m_weightFactor = 0.33f;

    [SerializeField]
    [Tooltip("The base weight for robots is 60 kg, the factor is the divide factor to modify this weight")]
    float m_impactMultiplier = 2;


    // Use this for initialization
    void Start () {
        m_aBodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody body in m_aBodies)
        {
            body.isKinematic = true;
            body.mass /= m_weightFactor;
        }

        m_animator = GetComponent<Animator>();
        m_follower = GetComponentInParent<NavigationFollower>();

    }


    public void Trigger(Vector3 force)
    {
        gameObject.layer = 9;
        foreach (Rigidbody body in m_aBodies)
        {
            body.gameObject.layer = 9;
            body.isKinematic = false;
            body.AddForce(force * m_impactMultiplier,ForceMode.Impulse);
        }

        if (m_animator != null)
        {
            m_animator.enabled = false;
        }

        if (m_follower != null)
            m_follower.enabled = false;
    }
}
