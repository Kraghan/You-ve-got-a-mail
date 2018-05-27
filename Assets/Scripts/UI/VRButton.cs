using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VRButton : VRInteractible
{

    protected Animator m_animator;

    protected Button m_button;

    protected Sprite m_normalSprite;

	// Use this for initialization
	void Start ()
    {
        m_button = GetComponent<Button>();
        m_normalSprite = m_button.image.sprite;

        m_animator = GetComponent<Animator>();
	}

    protected override void DoOnJustHover()
    {
        if (m_animator)
        {
            m_animator.SetBool("Hover", true);
            m_animator.SetBool("Pressed", false);
        }
        else
            m_button.image.sprite = m_button.spriteState.highlightedSprite;
    }

    protected override void DoOnJustNormal()
    {
        if (m_animator)
        {
            m_animator.SetBool("Hover", false);
            m_animator.SetBool("Pressed", false);
        }
        else
            m_button.image.sprite = m_normalSprite;
    }

    protected override void DoOnJustPressed()
    {
        if (m_animator)
        {
            m_animator.SetBool("Hover", false);
            m_animator.SetBool("Pressed", true);
        }
        else
            m_button.image.sprite = m_button.spriteState.pressedSprite;

        m_button.onClick.Invoke();
    }
}
