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
    private float m_speedTreshold;
    [SerializeField]
    private float m_controllerSpeedTreshold;
    [SerializeField]
    private GameObject m_blackScreen;

    private uint m_nbCollision = 0;

    private bool m_crashed = false;

	void Start ()
    {
        m_body = GetComponent<Rigidbody>();
        m_controller = GetComponent<BicycleController>();
	}
	
	void Update ()
    {
        if (m_crashed && m_controller.GetMotorInput() < 0.1 && m_nbCollision == 0)
            m_crashed = false;

        m_blackScreen.SetActive(m_crashed);
    }

    public bool IsCrashed()
    {
        return m_crashed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        m_nbCollision++;
        if (m_body.velocity.sqrMagnitude >= m_speedTreshold * m_speedTreshold && m_controller.GetMotorInput() >= m_controllerSpeedTreshold)
        {
            m_crashed = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        m_nbCollision--;
    }
}
