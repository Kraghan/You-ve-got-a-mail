using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider m_wheel;
    public bool m_motor;
    public bool m_steering;
    public Transform m_objectToRotateWith;

    public void ApplyLocalPositionToVisuals()
    {
        if (m_wheel.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = m_wheel.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        m_wheel.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
        
    }

}

public class BikeController : MonoBehaviour
{
    public List<AxleInfo> m_axleInfos;
    public float m_maxMotorTorque;
    public float m_maxSteeringAngle;
    private float m_steeringRatio;
    private float m_motorTorqueRatio;

    public void FixedUpdate()
    {
        float motor = m_maxMotorTorque * m_motorTorqueRatio;
        float steering = m_maxSteeringAngle * m_steeringRatio;

        foreach (AxleInfo axleInfo in m_axleInfos)
        {
            if (axleInfo.m_steering)
            {
                axleInfo.m_wheel.steerAngle = steering;
            }
            if (axleInfo.m_motor)
            {
                axleInfo.m_wheel.motorTorque = motor;
            }
            axleInfo.ApplyLocalPositionToVisuals();
        }
    }

    public void SetSteeringRatio(float steering)
    {
        m_steeringRatio = steering;
    }

    public void SetMotorTorqueRatio(float motorTorque)
    {
        m_motorTorqueRatio = motorTorque;
    }

    public float GetMotorTorqueRatio()
    {
        return m_motorTorqueRatio;
    }

    public float GetSteeringRatio()
    {
        return m_steeringRatio;
    }
}