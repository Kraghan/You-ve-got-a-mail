using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitation : MonoBehaviour
{
    [SerializeField]
    float m_distance;
    [SerializeField]
    float m_cycleTime;

    float m_previousPosition;
    float m_timeElapsed;


    private void Start()
    {
        m_previousPosition = 0;
        m_timeElapsed = 0;
    }

    void Update()
    {
        m_timeElapsed += Time.deltaTime;
        float y = Mathf.SmoothStep(-m_distance, m_distance, Mathf.PingPong(m_timeElapsed, m_cycleTime) / m_cycleTime);

        transform.position = new Vector3(transform.position.x, transform.position.y - m_previousPosition + y, transform.position.z);

        m_previousPosition = y;
    }
}
