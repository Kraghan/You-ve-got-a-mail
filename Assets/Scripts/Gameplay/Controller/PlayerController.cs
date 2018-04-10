using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArduinoConnect))]
[RequireComponent(typeof(BicycleController))]
public class PlayerController : MonoBehaviour {
    
    [Header("Handlebar")]
    [SerializeField]
    private Transform m_steerWheel;
    private ArduinoConnect m_arduino;
    [SerializeField]
    private GameObject m_handleBar;
    [SerializeField]
    private bool m_inverted;

    [Header("Motor wheel")]
    [SerializeField]
    private uint m_maxRPM;
    [SerializeField]
    private Timer m_updateRateSpeed;

    private float m_speed;
    private float m_nextTargetSpeed;
    private BicycleController m_bikeController;

    void Start()
    {   
        m_arduino = GetComponent<ArduinoConnect>();
        m_bikeController = GetComponent<BicycleController>();
        m_updateRateSpeed.Start();
    }

	// Update is called once per frame
	void Update ()
    {
        SpeedManagerArduino();
        OrientationManagerArduino();

    }

    void SpeedManagerArduino()
    {
        m_updateRateSpeed.UpdateTimer();
        if(m_updateRateSpeed.IsTimedOut())
        {
            m_updateRateSpeed.Restart();
            m_arduino.WriteToArduino("GET_WHEELSPEED");

            string read = m_arduino.ReadFromArduino(2);
            int rotationSinceLastCheck;
            if (!int.TryParse(read, out rotationSinceLastCheck))
            {
                Debug.LogWarning("Unable to convert data" + read);
                return;
            }
            //m_speed = m_bikeController.GetMotorTorqueRatio();
            m_nextTargetSpeed = Mathf.Clamp(rotationSinceLastCheck, 0, m_maxRPM) / m_maxRPM;
        }

        float actualSpeed = Mathf.Lerp(m_speed, m_nextTargetSpeed, m_updateRateSpeed.GetRatio());
        //Debug.Log(actualSpeed);
        m_bikeController.SetMotorInput(actualSpeed);

    }

    void OrientationManagerArduino()
    {
        m_arduino.WriteToArduino("GET_HANDLEBAR");

        string read = m_arduino.ReadFromArduino(2);
        float ratio;
        if (!float.TryParse(read, out ratio))
        {
            Debug.LogWarning("Unable to convert data"+read);
            return;
        }
        float positionPotentiometer = (ratio - 0.5f) * 2;
        if (m_inverted)
            positionPotentiometer *= -1;

        m_bikeController.SetSteerInput(positionPotentiometer);
    }
}
