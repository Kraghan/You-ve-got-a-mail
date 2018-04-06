using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerVR : MonoBehaviour {
    [SerializeField]
    private SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    private SteamVR_TrackedObject m_controllerRight;
    
    [Header("Handlebar")]
    [SerializeField]
    private Transform m_steerWheel;
    [SerializeField]
    private GameObject m_handleBar;
    [SerializeField]
    private bool m_inverted;

    private float m_speed = 0;
    private float m_rotation = 0;
    private float m_nextTargetSpeed;
    private BikeController m_bikeController;
    
    void Start()
    {
        m_bikeController = GetComponent<BikeController>();
    }

	// Update is called once per frame
	void Update ()
    {
        SpeedManagerVRController();
        OrientationManagerVRController();

        m_bikeController.SetMotorTorqueRatio(m_speed);
        m_bikeController.SetSteeringRatio(m_rotation);
	}

    void SpeedManagerVRController()
    {
        if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerLeft.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.y > 0.7f)
            {
                m_speed += 0.5f * Time.deltaTime;
            }

            else if (touchpad.y < -0.7f)
            {
                m_speed -= 1.0f * Time.deltaTime;
            }
        }
        else if (SteamVR_Controller.Input((int)m_controllerRight.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerRight.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.y > 0.7f)
            {
                m_speed += 0.5f * Time.deltaTime;
            }

            else if (touchpad.y < -0.7f)
            {
                m_speed -= 1.0f * Time.deltaTime;
            }
        }
        else
            m_speed -= 0.5f * Time.deltaTime;
        m_speed = Mathf.Clamp01(m_speed);
    }

    void OrientationManagerVRController()
    {
        if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerLeft.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            m_rotation = touchpad.x;
        }
        else if (SteamVR_Controller.Input((int)m_controllerRight.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerRight.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            m_rotation = touchpad.x;
        }
        else
            m_rotation = 0;
        m_rotation = Mathf.Clamp(m_rotation, -1, 1);
    }

}
