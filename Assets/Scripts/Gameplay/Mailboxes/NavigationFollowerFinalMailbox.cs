using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationFollowerFinalMailbox : NavigationFollower {

    VacuumMailBox m_mailbox;

    protected override void Start()
    {
        base.Start();

        m_mailbox = GetComponentInChildren<VacuumMailBox>();
    }

    protected override void DoWhenReachTarget()
    {
		
        if(m_target != null)
            m_mailbox.SetAsCurrentTarget();
		
    }

	protected override void Stop_Mailbox () {
		
		if ((m_target.IsStopPoint) && (!m_mailbox.IsTempDelivered ())) {
			m_stoppoint = true;
			if (m_target.IsFinalPoint) {
				m_mailbox.IsPursuit = false;
			}
		} else {
			m_stoppoint = false;
			m_mailbox.SetDelivered (false);
		}
	}
}
