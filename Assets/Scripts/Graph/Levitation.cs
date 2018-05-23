using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{
    [SerializeField]
    float m_distance;
    [SerializeField]
    float m_cycleTime;

    Vector3 m_initialPosition;
    float m_timeElapsed;


    private void Start()
    {
        m_initialPosition = transform.position;
        m_timeElapsed = 0;
    }

    void Update()
    {
        m_timeElapsed += Time.deltaTime;
        transform.position = m_initialPosition + Mathf.SmoothStep(-m_distance, m_distance, Mathf.PingPong(m_timeElapsed, m_cycleTime) / m_cycleTime) * Vector3.up;
    }
}
