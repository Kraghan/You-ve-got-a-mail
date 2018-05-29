using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVREnabler : MonoBehaviour {
    
    Camera m_camera;
    [SerializeField]
    float m_enabledDistance;

    VRActivateOnSight m_UIInSight;

    private void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update ()
    {
        RaycastHit hit;
        if(Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hit, m_enabledDistance, ~LayerMask.NameToLayer("UI")))
        {
            VRActivateOnSight UIToEnable = hit.collider.GetComponentInChildren<VRActivateOnSight>();
            if(UIToEnable)
            {
                if (m_UIInSight)
                    m_UIInSight.Disable();
                m_UIInSight = UIToEnable;
                m_UIInSight.Enable();
            }
        }
        else
        {
            if (m_UIInSight)
            {
                m_UIInSight.Disable();
                m_UIInSight = null;
            }
        }
	}
}
