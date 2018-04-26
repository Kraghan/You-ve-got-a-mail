using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSRenderer : MonoBehaviour {

    Text m_text;
    [SerializeField]
    Timer m_timer;
    uint m_fps;

    private void Start()
    {
        m_fps = 0;
        m_text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update ()
    {
        m_fps++;

        m_timer.UpdateTimer();
        if(m_timer.IsTimedOut())
        {
            m_text.text = "FPS : " + m_fps / m_timer.GetTimeToReach();
            m_timer.Restart();
            m_fps = 0;
        }
		
	}
}
