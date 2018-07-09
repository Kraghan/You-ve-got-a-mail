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
	private bool m_panne = false;

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
		if (!m_panne) {
			float vertical = Input.GetAxis ("Vertical");

			if (vertical > 0.7f) {
				m_speed += vertical * Time.deltaTime;
			} else if (vertical < 0f) {
				m_speed += 4 * vertical * Time.deltaTime;
			} else
				m_speed -= 0.5f * Time.deltaTime;
			m_speed = Mathf.Clamp01 (m_speed);
		} else {
			m_speed = 0;
		}
    }

    void OrientationManagerVRController()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
		m_rotation = Mathf.Lerp(m_rotation, horizontal, 8f * Time.deltaTime);        
        m_rotation = Mathf.Clamp(m_rotation, -1, 1);

    }

	public void SetMouseSensibility (Slider slider) {

		god.XSensitivity = slider.value;
		god.YSensitivity = slider.value;

		//Je crée le fichier de sauvegarde
		PlayerPrefs.SetFloat("MouseSensibility", slider.value);

	}

	public void SetPanne (bool panne) {

		m_panne = panne;

	}
}
