using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
[RequireComponent(typeof(LineRenderer))]
public class VRInteractibleController : MonoBehaviour
{
    SteamVR_TrackedObject m_trackedObject;
    [SerializeField]
    float m_enabledDistance = 1;
    [SerializeField]
    LineRenderer m_lineRenderer;
    [SerializeField]
    Transform m_startLaserPoint;
    Animator m_model;

    VRInteractible m_uiElement;
    
	// Use this for initialization
	void Start ()
    {
        m_trackedObject = GetComponent<SteamVR_TrackedObject>();
        m_lineRenderer.positionCount = 2;
        m_model = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        m_lineRenderer.SetPosition(0, m_startLaserPoint.position);
        if (!m_model.GetBool("Grab") && !m_model.GetBool("Gun") 
            && Physics.Raycast(m_startLaserPoint.position, m_startLaserPoint.forward, out hit, m_enabledDistance, ~LayerMask.NameToLayer("UI")))
        {

            m_model.SetBool("Pointing", true);
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
                if(SteamVR_Controller.Input((int)m_trackedObject.index).GetHairTrigger())
                {
                    m_uiElement.SetPressed();
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
