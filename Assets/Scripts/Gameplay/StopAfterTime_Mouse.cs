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

	MouseMailCanon Mouse_Mail_Canon;

	private Endings theend;

	private float letime;
	private bool doonce;

    private void Start()
    {
        m_controller = GetComponent<KeybordController>();
		theend = GetComponent<Endings> ();
    }

    // Update is called once per frame
    void Update ()
    {
		if((m_manager.GetTime() >= 600) && (m_manager.Mode_selection.m_defaultPlayMode == Mode_selector.MyPlayMode.POINTS))
        {
            m_controller.SetPanne(true);
            m_uiToDisable.SetActive(false);
            m_uiToEnable.SetActive(true);

			Mouse_Mail_Canon = m_controller.gameObject.GetComponentInChildren<MouseMailCanon> ();
			Mouse_Mail_Canon.enabled = false;

			letime += Time.deltaTime;

			if ((letime >= 5f) && (!doonce)) {
				theend.PointsEnding ();
				doonce = true;
			}
        }
	}
}
