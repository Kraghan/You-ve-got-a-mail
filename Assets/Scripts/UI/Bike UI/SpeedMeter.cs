using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField]
    public RectTransform m_transform;
    [SerializeField]
    float m_minAngle = 110;
    [SerializeField]
    float m_maxAngle = -110;
    [SerializeField]
    BicycleController m_bikeController;
    

    // Use this for initialization
    void Start ()
    {
        AkSoundEngine.PostEvent("YGM_Bike_Start", m_bikeController.gameObject);
        m_transform.localRotation = Quaternion.Euler(m_transform.localRotation.eulerAngles.x, m_transform.localRotation.y, m_minAngle);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        float angle = Mathf.Lerp(m_minAngle, m_maxAngle, m_bikeController.FrontWheelCollider.rpm / 700);
        
        m_transform.localRotation = Quaternion.Euler(m_transform.localRotation.eulerAngles.x, m_transform.localRotation.y, angle);

        AkSoundEngine.SetRTPCValue("YGM_BIKESPEED", m_bikeController.FrontWheelCollider.rpm / 700);
    }
}
