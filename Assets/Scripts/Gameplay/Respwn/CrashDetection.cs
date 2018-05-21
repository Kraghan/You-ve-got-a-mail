using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BicycleController))]
[RequireComponent(typeof(Collider))]
public class CrashDetection : MonoBehaviour
{
    private Rigidbody m_body;
    private BicycleController m_controller;

    private bool m_crashed = false;
    [SerializeField]
    private SafePylone m_lastRespawnPylone;

    [SerializeField]
    Timer m_timeToFade;
    [SerializeField]
    Texture2D m_texture;
    [SerializeField]
    ParticleSystem m_particles;

    void Start ()
    {
        m_controller = GetComponent<BicycleController>();
        m_timeToFade.Start();
	}
	
	void Update ()
    {
        if (m_crashed)
        {
            if(m_controller.GetMotorInput() < 0.1)
            {
                m_crashed = false;
                m_lastRespawnPylone.Respawn();
                m_timeToFade.Restart();
                m_particles.Play();
            }
            else
            {
                m_timeToFade.UpdateTimer();
            }
        }
    }

    public bool IsCrashed()
    {
        return m_crashed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Vector3 normalWall = Vector3.zero;

        for(uint i = 0; i < collision.contacts.Length; ++i)
        {
            normalWall += collision.contacts[i].normal;
        }
        normalWall.Normalize();
        
        float angle = Mathf.Abs(Vector3.SignedAngle(normalWall, Quaternion.AngleAxis(180,Vector3.up) * transform.forward, Vector3.up));

        if (angle < 45)
            m_crashed = true;
    }

    public void SetRespawnPylone(SafePylone pylone)
    {
        if (m_lastRespawnPylone != null)
            m_lastRespawnPylone.SetInactive();
        m_lastRespawnPylone = pylone;
        m_lastRespawnPylone.SetActive();

    }

    private void OnGUI()
    {

        float alpha = m_timeToFade.GetRatio();

        GUI.color = new Color(0,0,0,alpha);
    
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),m_texture);
    }
}
