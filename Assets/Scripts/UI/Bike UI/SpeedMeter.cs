using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField]
    RectTransform m_transform;
    [SerializeField]
    float m_minAngle = 110;
    [SerializeField]
    float m_maxAngle = -110;
    [SerializeField]
    BicycleController m_bikeController;
    [SerializeField]
    float m_maxAnglePerSecond = 30;

    float m_previousAngle = 0;

    // Use this for initialization
    void Start ()
    {
        m_transform.localRotation = Quaternion.Euler(m_transform.localRotation.eulerAngles.x, m_transform.localRotation.y, m_minAngle);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        float angle = Mathf.Lerp(m_minAngle, m_maxAngle, m_bikeController.GetMotorInput());

        if (Mathf.DeltaAngle(m_previousAngle, angle) > m_maxAnglePerSecond * Time.deltaTime)
        {
            m_transform.localRotation = Quaternion.Euler(m_transform.localRotation.eulerAngles.x, m_transform.localRotation.y, m_previousAngle + m_maxAnglePerSecond * Time.deltaTime);
            m_previousAngle += m_maxAnglePerSecond * Time.deltaTime;
        }
        else
        {
            m_transform.localRotation = Quaternion.Euler(m_transform.localRotation.eulerAngles.x, m_transform.localRotation.y, angle);
            m_previousAngle = angle;
        }
    }
}
