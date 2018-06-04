using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer {
    
    float m_timeElapsed = 0;
    [SerializeField]
    float m_timeToReach = 2;

    public void Start(float time = 0)
    {
        if (time != 0)
            m_timeToReach = time;
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

    public float GetRatio()
    {
        return m_timeElapsed / m_timeToReach;
    }

    public float GetTimeToReach()
    {
        return m_timeToReach;
    }
}
