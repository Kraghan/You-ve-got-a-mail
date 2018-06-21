using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLongButton : VRButton
{
    [SerializeField]
    Timer m_timeToMaintain;
    [SerializeField]
    RectTransform m_cursorTransform;

    bool m_triggered = false;

    protected override void DoOnJustPressed()
    {
        m_timeToMaintain.Restart();
    }

    protected override void DoOnPressed()
    {
        if (m_triggered)
            return;

        m_timeToMaintain.UpdateTimer();
		if (!m_timeToMaintain.IsTimedOut())
        	m_cursorTransform.rotation = Quaternion.Euler(m_cursorTransform.rotation.eulerAngles.x, m_cursorTransform.rotation.eulerAngles.y, Mathf.Lerp(0, 360, m_timeToMaintain.GetRatio()));
		
        if (m_timeToMaintain.IsTimedOut())
        {
            m_button.onClick.Invoke();
            m_triggered = true;
        }
    }

    protected override void DoOnJustNormal()
    {
        base.DoOnJustNormal();
        m_timeToMaintain.Restart();

        m_cursorTransform.rotation = Quaternion.Euler(m_cursorTransform.rotation.eulerAngles.x, m_cursorTransform.rotation.eulerAngles.y, 0);

        m_triggered = false;
    }

    protected override void DoOnJustHover()
    {
        base.DoOnJustHover();
        m_timeToMaintain.Restart();

        m_cursorTransform.rotation = Quaternion.Euler(m_cursorTransform.rotation.eulerAngles.x, m_cursorTransform.rotation.eulerAngles.y, 0);
    }
}
