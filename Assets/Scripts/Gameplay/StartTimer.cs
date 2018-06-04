using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StartTimer : MonoBehaviour {

    [SerializeField]
    ScoreManager m_scoreManager;

    [SerializeField]
    bool m_stopTimerOnEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!m_stopTimerOnEnter)
                m_scoreManager.StartTimer();
            else
                m_scoreManager.StopTimer();
        }
    }
}
