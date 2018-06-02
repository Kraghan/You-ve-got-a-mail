using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractible : MonoBehaviour {

    bool m_hover = false;
    bool m_pressed = false;

    public bool IsHover()
    {
        return m_hover;
    }

    public bool IsPressed()
    {
        return m_pressed;
    }

    public void SetHover()
    {
        if (!m_hover)
        {
            DoOnJustHover();
        }
        else
            DoOnHover();

        m_pressed = false;
        m_hover = true;
    }

    public void SetPressed(bool justPressedButton)
    {
        if (!m_pressed && justPressedButton)
        {
            DoOnJustPressed();
        }
        else
            DoOnPressed();

        m_pressed = true;
        m_hover = false;
    }

    public void SetNormal()
    {
        if (!m_hover && !m_pressed)
        {
            DoOnNormal();
        }
        else
            DoOnJustNormal();

        m_pressed = false;
        m_hover = false;
    }

    protected virtual void DoOnPressed()
    {

    }

    protected virtual void DoOnHover()
    {

    }

    protected virtual void DoOnJustPressed()
    {

    }

    protected virtual void DoOnJustHover()
    {

    }

    protected virtual void DoOnJustNormal()
    {

    }

    protected virtual void DoOnNormal()
    {

    }

}
