using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MouseInteractibleController : MonoBehaviour
{
    [SerializeField]
    float m_enabledDistance = 1;
    [SerializeField]
    LineRenderer m_lineRenderer;
    [SerializeField]
    Transform m_startLaserPoint;
    Animator m_model;

    VRInteractible m_uiElement;
	public LayerMask thelayer;
    
	// Use this for initialization
	void Start ()
    {
        m_lineRenderer.positionCount = 2;
        m_model = GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        m_lineRenderer.SetPosition(0, m_startLaserPoint.position);
        if (!m_model.GetBool("Grab") && !m_model.GetBool("Gun") 
            && Physics.Raycast(m_startLaserPoint.position, m_startLaserPoint.forward, out hit, m_enabledDistance, thelayer))
        {

            m_model.SetBool("Pointing", true);

            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(1, hit.point);
            m_lineRenderer.enabled = true;
            VRInteractible UiElement = hit.collider.GetComponent<VRInteractible>();
            if (UiElement)
            {
                if (m_uiElement && UiElement.GetInstanceID() != m_uiElement.GetInstanceID())
                {
                    m_uiElement.SetNormal();
                }

                m_uiElement = UiElement;
				if(Input.GetButton("Trigger"))
                {
					m_uiElement.SetPressed(Input.GetButtonDown("Trigger"));
                }
                else
                {
                    m_uiElement.SetHover();
                }

            }
            else
            {
                if (m_uiElement)
                    m_uiElement.SetNormal();
            }
        }
        else
        {
            m_model.SetBool("Pointing", false);
            if (m_uiElement)
                m_uiElement.SetNormal();
            m_lineRenderer.enabled = false;

        }
    }
}
