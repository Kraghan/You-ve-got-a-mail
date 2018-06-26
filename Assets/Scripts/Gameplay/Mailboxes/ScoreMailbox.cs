using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VacuumMailBox))]
public class ScoreMailbox : MonoBehaviour {
    public static float s_score = 0;
	public static int s_scorepoints = 0;
	public static int s_totalmailbox = 0;
    [SerializeField]
    float m_secondsScore = 5;
	public int m_Score = 1000;
    VacuumMailBox m_mailbox;
    bool m_alreadyAdded = false;

    private void Start()
    {
        m_mailbox = GetComponent<VacuumMailBox>();
    }

    // Update is called once per frame
    void Update ()
    {

		if(m_mailbox.IsDelivered() && !m_alreadyAdded)
        {
			s_totalmailbox  ++;
			s_scorepoints += m_Score;
            s_score += m_secondsScore;
            m_alreadyAdded = true;
        }
	}
}
