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

    [SerializeField]
    private GameObject m_blackScreen;

    private bool m_crashed = false;
    [SerializeField]
    private SafePylone m_lastRespawnPylone;

    void Start ()
    {
        m_controller = GetComponent<BicycleController>();
	}
	
	void Update ()
    {
        if (m_crashed && m_controller.GetMotorInput() < 0.1)
        {
            m_crashed = false;
            m_lastRespawnPylone.Respawn();
        }

        m_blackScreen.SetActive(m_crashed);
        
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
        m_lastRespawnPylone = pylone;
    }
}
