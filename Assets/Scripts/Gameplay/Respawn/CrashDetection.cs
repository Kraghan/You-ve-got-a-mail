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
    ParticleSystem m_particles;

    [SerializeField]
    Camera m_camera;

    int m_oldLayerMask;
    Color m_oldColor;

    uint m_collisionCount = 0;

    void Start ()
    {
        m_controller = GetComponent<BicycleController>();
        m_oldLayerMask = m_camera.cullingMask;
        m_oldColor = m_camera.backgroundColor;
	}
	
	void Update ()
    {
        if (m_crashed)
        {
            if(m_collisionCount == 0)
            {
                BackToNormal();
                m_crashed = false;
            }
            else if(m_controller.GetMotorInput() < 0.1)
            {
                Respawn();
            }
        }
    }

    public bool IsCrashed()
    {
        return m_crashed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mover") && m_controller.GetMotorInput() >= 0.1f)
        {
            RagdollTriggerer triggerer = other.gameObject.GetComponentInParent<RagdollTriggerer>();
            if (triggerer != null)
            {
                triggerer.Trigger(m_controller.GetMotorInput() * transform.forward, other.gameObject.GetInstanceID());

                return;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        m_collisionCount++;
        Vector3 normalWall = Vector3.zero;

        for(uint i = 0; i < collision.contacts.Length; ++i)
        {
            normalWall += collision.contacts[i].normal;
        }
        normalWall.Normalize();
        
        float angle = Mathf.Abs(Vector3.SignedAngle(normalWall, Quaternion.AngleAxis(180,Vector3.up) * transform.forward, Vector3.up));

        if (angle < 45)
        {
            m_crashed = true;
            BlackScreen();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        m_collisionCount--;
    }

    public void SetRespawnPylone(SafePylone pylone)
    {
        if (m_lastRespawnPylone != null)
            m_lastRespawnPylone.SetInactive();
        m_lastRespawnPylone = pylone;
        m_lastRespawnPylone.SetActive();

    }

    void BlackScreen()
    {
        m_camera.cullingMask = (1 << LayerMask.NameToLayer("Nothing"));
        m_camera.clearFlags = CameraClearFlags.SolidColor;
        m_camera.backgroundColor = Color.black;
    }

    void BackToNormal()
    {
        m_camera.cullingMask = m_oldLayerMask;
        m_camera.clearFlags = CameraClearFlags.Skybox;
        m_camera.backgroundColor = m_oldColor;
    }

    public void Respawn()
    {
        m_lastRespawnPylone.Respawn();
        m_particles.Play();
        BackToNormal();
        m_crashed = false;
    }
}
