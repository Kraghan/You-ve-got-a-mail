using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAfterTime_Mouse : MonoBehaviour {

    [SerializeField]
    ScoreManager m_manager;
	KeybordController m_controller;
    [SerializeField]
    GameObject m_uiToDisable;
    [SerializeField]
    GameObject m_uiToEnable;

    private void Start()
    {
        m_controller = GetComponent<KeybordController>();
    }

    // Update is called once per frame
    void Update ()
    {
		if((m_manager.GetTime() >= 600) && (m_manager.Mode_selection.m_defaultPlayMode == Mode_selector.MyPlayMode.POINTS))
        {
            m_controller.SetPanne(true);
            m_uiToDisable.SetActive(false);
            m_uiToEnable.SetActive(true);
        }
	}
}
