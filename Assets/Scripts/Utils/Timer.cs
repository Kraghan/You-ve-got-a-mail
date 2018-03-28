using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    
    float m_timeElapsed = 0;
    [SerializeField]
    float m_timeToReach = 2;

    public void Start()
    {
        m_timeElapsed = 0;
    }

    public void Restart()
    {
        m_timeElapsed = 0;
    }

    public void UpdateTimer()
    {
        m_timeElapsed += Time.deltaTime;
    }

    public void FixedUpdateTimer()
    {
        m_timeElapsed += Time.fixedDeltaTime;
    }

    public bool IsTimedOut()
    {
        return m_timeElapsed >= m_timeToReach;
    }
}
