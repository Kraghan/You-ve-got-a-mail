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

    bool m_triggered = false;

    [SerializeField]
    Timer m_timeBeforeDestroy;
    [SerializeField]
    GameObject m_particlesModel;
    ParticleSystem m_destroyParticles;
    Timer m_particleLifeTime = new Timer();

    Spawn_Random m_spawner;

    bool m_particleStarted = false;

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

    public void Update()
    {
        if (!m_triggered)
            return;

        m_timeBeforeDestroy.UpdateTimer();

        if(m_timeBeforeDestroy.IsTimedOut())
        {
            m_particleLifeTime.UpdateTimer();
            float value = Mathf.Lerp(1, 0, m_particleLifeTime.GetRatio());
            transform.localScale = new Vector3(value, value, value);

            if (!m_destroyParticles.isEmitting && m_particleStarted)
            {
                if(m_spawner)
                    m_spawner.CreatePedestrian();
                Destroy(transform.parent.gameObject);
            }

            if (!m_particleStarted)
            {
                m_destroyParticles.Play();
                m_particleStarted = true;
            }
        }
    }


    public void Trigger(Vector3 force)
    {
        if (m_triggered)
            return;

        gameObject.layer = 9;
        foreach (Rigidbody body in m_aBodies)
        {
            body.gameObject.layer = 9;
            body.isKinematic = false;
            //body.AddForce(force * m_impactMultiplier,ForceMode.Impulse);
        }

        if (m_animator != null)
        {
            m_animator.enabled = false;
        }

        if (m_follower != null)
            m_follower.enabled = false;

        m_triggered = true;

        m_timeBeforeDestroy.Restart();
        m_destroyParticles = Instantiate(m_particlesModel).GetComponent<ParticleSystem>();
        m_destroyParticles.transform.position = gameObject.transform.position;
        m_destroyParticles.Stop();

        m_particleLifeTime.Start(m_destroyParticles.main.duration);

        AkSoundEngine.PostEvent("YGM_Crash", gameObject);
    }

    public void SetSpawner(Spawn_Random sp)
    {
        m_spawner = sp;
    }
}
