using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAfterTime : MonoBehaviour {

    [SerializeField]
    ScoreManager m_manager;
    PlayerController m_controller;
    [SerializeField]
    GameObject m_uiToDisable;
    [SerializeField]
    GameObject m_uiToEnable;

	MailCanon[] Mail_Canon;
	MailController[] Mail_Controller;

	private Endings theend;

	private float letime;
	private bool doonce, onceagainonce;

    private void Start()
    {
        m_controller = GetComponent<PlayerController>();
		theend = GetComponent<Endings> ();
    }

    // Update is called once per frame
    void Update ()
    {
		if((m_manager.GetTime() >= 600) && (Mode_selector.m_defaultPlayMode == Mode_selector.MyPlayMode.POINTS))
        {
			if (!onceagainonce) {
				onceagainonce = true;

				m_controller.SetPanne (true);
				m_uiToDisable.SetActive (false);
				m_uiToEnable.SetActive (true);

			}

				Mail_Canon = m_controller.gameObject.GetComponentsInChildren<MailCanon> ();
				Mail_Controller = m_controller.gameObject.GetComponentsInChildren<MailController> ();

				foreach (MailCanon lecanon in Mail_Canon)
					lecanon.enabled = false;
				foreach (MailController lelancer in Mail_Controller)
					lelancer.enabled = false;

			letime += Time.deltaTime;

			if ((letime >= 5f) && (!doonce)) {
				theend.PointsEnding ();
				doonce = true;
			} 
        }
	}
}
