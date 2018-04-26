using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BicycleController))]
public class PlayerController : MonoBehaviour {
    
    [Header("Handlebar")]
    [SerializeField]
    private Transform m_steerWheel;
    [SerializeField]
    private GameObject m_handleBar;
    [SerializeField]
    private bool m_inverted;

    [Header("Motor wheel")]
    [SerializeField]
    private float m_maxRPM;
    [SerializeField]
    private Timer m_updateRateSpeed;

    [SerializeField]
    private int m_timeOutArduino = 5;

    private float m_speed = float.NaN;
    private float m_nextTargetSpeed;
    private BicycleController m_bikeController;

    private float m_startAngle = float.NaN;

    private List<int> m_speedStorage = new List<int>();
    private List<float> m_previousSpeed = new List<float>();

    [Header("Arduino")]
    [SerializeField]
    private int m_baudRate = 250000;

    [SerializeField]
    private int m_readTimeout = 20;

    [SerializeField]
    private int m_queueLenght = 1;

    [SerializeField]
    private string m_portName = "COM5";

    wrmhl m_arduino = new wrmhl();

    void Start()
    {   
        m_bikeController = GetComponent<BicycleController>();
        m_updateRateSpeed.Start();

        m_arduino.set(m_portName, m_baudRate, m_readTimeout, m_queueLenght);
        m_arduino.connect();
        m_arduino.send("Calibrate");
    }

    void OnApplicationQuit()
    {
        m_arduino.close();
    }

    // Update is called once per frame
    void Update ()
    {
        string data = m_arduino.readQueue();
        string previousData = null;

        while (data != null)
        {
            previousData = data;
            data = m_arduino.readQueue();
        }
        data = previousData;

        if (data == null)
            return;

        string[] dataSplitted = data.Split(';');
        float rotation;
        if(float.TryParse(dataSplitted[0],out rotation))
            OrientationManagerArduino(rotation);
        int speed;
        if(int.TryParse(dataSplitted[1], out speed))
            SpeedManagerArduino(speed);
    }

    void SpeedManagerArduino(int rotationSinceLastCheck)
    {
        m_speedStorage.Add(rotationSinceLastCheck);
        while (m_speedStorage.Count * m_updateRateSpeed.GetTimeToReach() > 0.25)
            m_speedStorage.RemoveAt(0);

        int rotationInHalfSecond = 0;
        foreach (int value in m_speedStorage)
            rotationInHalfSecond += value;
            
        float rotation = Mathf.Clamp(rotationInHalfSecond * 60, 0, m_maxRPM);

        m_previousSpeed.Add(rotation);
        if (m_previousSpeed.Count >= 10)
            m_previousSpeed.RemoveAt(0);
        if (rotationInHalfSecond == 0)
        {
            float value = 0;
            for (int i = m_previousSpeed.Count - 1; i >= 0; --i)
            {
                if (m_previousSpeed[i] != 0)
                {
                    value = m_previousSpeed[i];
                    break;
                }
            }
            rotation = value;
        }

        //Debug.Log("RPM : " + rotation);

        m_bikeController.SetMotorInput(rotation / m_maxRPM);

    }

    void OrientationManagerArduino(float angle)
    {
        if(float.IsNaN(m_startAngle))
        {
            m_startAngle = angle;
            m_bikeController.SetSteerInput(0);
            return;
        }

        float angle2 = m_bikeController.SteerAngle / 2;
        Debug.Log(angle + " - " + m_startAngle + " = " + (m_startAngle - angle));
        Debug.Log(m_startAngle - angle);

        float ratio = Mathf.Lerp(-1, 1, ((angle - m_startAngle) + angle2) / m_bikeController.SteerAngle);
        if (m_inverted)
            ratio *= -1;
        m_bikeController.SetSteerInput(ratio);
    }
}
