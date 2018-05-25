using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProbabilityOfAppearenceOfItem
{
    public float m_probability;
    public Mail m_item;
}


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Mail : MonoBehaviour
{
    [SerializeField]
    protected string m_soundOnCollision = "";
    [SerializeField]
    protected Timer m_timeBeforeDestroy;

    protected bool m_hasCollide = false;

    public Mail()
    {
        m_timeBeforeDestroy = new Timer();
    }

	// Use this for initialization
	void Start ()
    {
        tag = "Mail";
	}

    private void Update()
    {
        if(m_hasCollide)
        {
            m_timeBeforeDestroy.UpdateTimer();
            if(m_timeBeforeDestroy.IsTimedOut())
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Mail"))
            return;
        if (m_hasCollide)
            return;
        //AkSoundEngine.PostEvent(m_soundOnCollision, gameObject);
        m_timeBeforeDestroy.Start();
        m_hasCollide = true;
    }
}
