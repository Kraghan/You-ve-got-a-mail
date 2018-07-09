using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VacuumMailBox))]
public class ScoreMailbox : MonoBehaviour {
    public static float s_score = 0;
	public static int s_scorepoints = 0;
	public static int s_totalmailbox = 0;
	public static float m_scoremultiplier = 1;
	public static float multi_timer, multi_time = 10;

    [SerializeField]
    float m_secondsScore = 5;
	public int m_Score = 1000;
    VacuumMailBox m_mailbox;
    public bool m_alreadyAdded = false;

	//fonction pour le restart
	private void Awake () {
		s_score = 0;
		s_scorepoints = 0;
		s_totalmailbox = 0;
		m_scoremultiplier = 1;
		multi_timer = 0;
		multi_time = 10;
	}

    private void Start()
    {
        m_mailbox = GetComponent<VacuumMailBox>();
    }

    // Update is called once per frame
    void Update ()
    {		

		if(m_mailbox.IsDelivered() && !m_alreadyAdded)
        {
			if (m_mailbox.tag != "Levier") {
				
				s_totalmailbox++;

			}

				s_scorepoints += (int)(m_Score * m_scoremultiplier);

				multi_timer = 0;
				if (m_scoremultiplier < 5) {
					multi_time -= 0.75f;
					m_scoremultiplier += 0.5f;
				}
			
				s_score += m_secondsScore;

            m_alreadyAdded = true;
        }
	}
}
