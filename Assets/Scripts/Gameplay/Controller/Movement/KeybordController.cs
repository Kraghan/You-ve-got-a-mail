using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybordController : MonoBehaviour {
    [SerializeField]
    private bool m_inverted;

    private float m_speed = 0;
    private float m_rotation = 0;
    private float m_nextTargetSpeed;
    private BicycleController m_bikeController;

	public God_move god;

    void Start()
    {
        m_bikeController = GetComponent<BicycleController>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedManagerVRController();
        OrientationManagerVRController();
        
        m_bikeController.SetMotorInput(m_speed);
        m_bikeController.SetSteerInput(m_rotation);
    }

    void SpeedManagerVRController()
    {
        float vertical = Input.GetAxis("Vertical");
        
        if (vertical > 0.7f)
        {
            m_speed += 0.5f * Time.deltaTime;
        }

        else if (vertical < -0.7f)
        {
            m_speed -= 1.0f * Time.deltaTime;
        }
        else
            m_speed -= 0.5f * Time.deltaTime;
        m_speed = Mathf.Clamp01(m_speed);
    }

    void OrientationManagerVRController()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0.2f)
        {
            m_rotation = horizontal;
        }
        else if (horizontal < -0.2f)
        {
            m_rotation = horizontal;
        }
        else
            m_rotation = 0;
        m_rotation = Mathf.Clamp(m_rotation, -1, 1);
    }

	public void SetMouseSensibility (Slider slider) {

		god.XSensitivity = slider.value;
		god.YSensitivity = slider.value;

	}
}
