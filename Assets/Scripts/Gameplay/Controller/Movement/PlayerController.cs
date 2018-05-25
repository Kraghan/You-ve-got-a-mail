using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulseWithTime
{
    public float m_time;
    public int m_nbImpulse;

    public ImpulseWithTime(float time, int nbImpulse)
    {
        m_time = time;
        m_nbImpulse = nbImpulse;
    }
}

[RequireComponent(typeof(BicycleController))]
public class PlayerController : MonoBehaviour {
    
    [Header("Handlebar")]
    [SerializeField]
    private bool m_inverted;
    [SerializeField]
    private float m_sensibility = 2;
    [SerializeField]
    private float m_middleCap = 0.05f;

    [Header("Motor wheel")]
    [SerializeField]
    private float m_impulsePerMagnet;
    [SerializeField]
    private float m_speedDecreaseRate;
    [SerializeField]
    private float m_maxRPM;

    private BicycleController m_bikeController;

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

    float timeSinceLastImpulse = 0;
    List<ImpulseWithTime> m_lastImpulses = new List<ImpulseWithTime>();
    List<float> m_lastRPM = new List<float>();
    void Start()
    {   
        m_bikeController = GetComponent<BicycleController>();

        m_arduino.set(m_portName, m_baudRate, m_readTimeout, m_queueLenght);
        m_arduino.connect();
    }

    void OnApplicationQuit()
    {
        m_arduino.close();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        string data = m_arduino.readQueue();
        string previousData = null;
        int activation = 0;
        while (data != null)
        {
            string[] splitted = data.Split(';');
            int value;
            if (int.TryParse(splitted[1], out value))
                activation+=value;
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
        int inFrontOfMagnet;
        if (int.TryParse(dataSplitted[1], out inFrontOfMagnet))
        {
            activation += inFrontOfMagnet;
            SpeedManagerArduino(activation);
        }
    }

    void SpeedManagerArduino(int nbImpulse)
    {
        float motorInput = m_bikeController.GetMotorInput();

        if (nbImpulse == 0)
            timeSinceLastImpulse += Time.fixedDeltaTime;
        else
        {
            timeSinceLastImpulse = 0;
        }

        m_lastImpulses.Add(new ImpulseWithTime(Time.fixedDeltaTime, nbImpulse));
        float RPM = CalculateRPM();
        m_lastRPM.Add(RPM);
        if (m_lastRPM.Count > 10)
            m_lastRPM.RemoveAt(0);

        // start impulse
        if (motorInput == 0)
        {
            for (int i = 0; i < nbImpulse; ++i)
            {
                motorInput += m_impulsePerMagnet;
            }
        }
        else
        {
            float rotations = Mathf.Clamp(RPM, 0, m_maxRPM);
            motorInput = rotations / m_maxRPM;
        }

        // Prevent unvolontary stop
        if(RPM == 0)
        {
            for(int i = m_lastRPM.Count - 1; i >= 0; i--)
            {

                if(m_lastRPM[i] != 0)
                {
                    motorInput = m_lastRPM[i];
                    break;
                }
            }
        }
        if(motorInput == 0)
        {
            m_lastImpulses.Clear();
        }

        motorInput = Mathf.Clamp01(motorInput);

        m_bikeController.SetMotorInput(motorInput);

    }

    void OrientationManagerArduino(float angle)
    {
        float angle2 = m_bikeController.SteerAngle / 2;

        float ratio = Mathf.Lerp(-1, 1, (angle / m_sensibility + angle2) / m_bikeController.SteerAngle);
        if (m_inverted)
            ratio *= -1;

        if (Mathf.Abs(ratio) < m_middleCap)
            ratio = 0;

        m_bikeController.SetSteerInput(ratio);
    }

    float CalculateRPM()
    {
        float rotations = 0;
        float timeUnit = 0.25f;

        // Clear oldest datas
        float timeStored = 0;
        for(int i = m_lastImpulses.Count - 1; i >= 0; --i)
        {
            if (timeStored >= timeUnit)
            {
                m_lastImpulses.RemoveRange(0, i + 1);
                break;
            }
            rotations += m_lastImpulses[i].m_nbImpulse;
            timeStored += m_lastImpulses[i].m_time;
        }

        return rotations ;
    }
}
