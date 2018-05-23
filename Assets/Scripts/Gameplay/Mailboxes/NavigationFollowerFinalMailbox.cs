using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationFollowerFinalMailbox : NavigationFollower {

    VacuumMailBox m_mailbox;

    protected override void Start()
    {
        base.Start();

        m_mailbox = m_target.GetComponentInChildren<VacuumMailBox>();
    }

    protected override bool ConditionToChooseNextTarget(float distance)
    {
        return base.ConditionToChooseNextTarget(distance) && m_mailbox.IsDelivered();
    }

    protected override void DoWhenReachTarget()
    {
		
        if(m_target != null)
            m_mailbox.SetAsCurrentTarget();
		
    }
}
