using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class BicycleController : MonoBehaviour
{

    private Rigidbody rigid;

    public WheelCollider FrontWheelCollider;
    public WheelCollider RearWheelCollider;
    public Transform FrontWheelTransform;
    public Transform RearWheelTransform;
    public Transform Fender;
    public Transform SteeringHandlebar;
    public Transform COM;

    //Gearbox
    public bool changingGear = false;
    public float gearShiftRate = 10.0f;
    [HideInInspector]
    public float[] gearSpeed;
    public int currentGear;
    public int totalGears = 6;
    private int _totalGears
    {
        get
        {
            return totalGears - 1;
        }
    }

    //Bike Body Lean
    public GameObject chassis;
    public float chassisVerticalLean = 4.0f;
    public float chassisHorizontalLean = 4.0f;
    private float horizontalLean = 0.0f;
    private float verticalLean = 0.0f;

    //Configurations
    [HideInInspector]
    public float EngineTorque = 1500f;
    public float MaxEngineRPM = 6000f;
    public float MinEngineRPM = 1000f;
    public float SteerAngle = 40f;
    [HideInInspector]
    public float Speed;
    public float highSpeedSteerAngle = 5f;
    public float highSpeedSteerAngleAtSpeed = 80f;
    public float maxSpeed = 180f;
    public float Brake = 2500f;

    private float EngineRPM = 0f;
    private float motorInput = 0f;
    private float defsteerAngle = 0f;
    private float RotationValue1 = 0f;
    private float RotationValue2 = 0f;
    [HideInInspector]
    public bool brakingNow = false;
    [HideInInspector]
    public float steerInput = 0f;
    [HideInInspector]
    public bool crashed = false;
    private bool reversing = false;

    public GameObject pedal; // Bike pedal


    void Start()
    {
        //Rigidbody
        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotationZ;
        rigid.centerOfMass = new Vector3(COM.localPosition.x * transform.localScale.x, COM.localPosition.y * transform.localScale.y, COM.localPosition.z * transform.localScale.z);
        rigid.maxAngularVelocity = 2f;

        defsteerAngle = SteerAngle;

    }

    void FixedUpdate()
    {

        Inputs();
        Engine();
        Braking();
        ShiftGears();

        Bike_Controller(); // my code
    }

    void Update()
    {

        WheelAlign();
        Lean();

    }

    public void SetMotorInput(float value)
    {
        motorInput = value;
    }

    public void SetSteerInput(float value)
    {
        steerInput = value;
    }

    void Inputs()
    {
        Speed = rigid.velocity.magnitude * 3.6f;

        //Freezing rotation by Z axis.
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        //Reverse bool
        if (motorInput < 0 && transform.InverseTransformDirection(rigid.velocity).z < 0)
            reversing = true;
        else
            reversing = false;

    }

    public void Bike_Controller()
    { // mycode

        if(pedal)
            if (0 < Speed)
                pedal.transform.Rotate(Speed * 10f * Time.deltaTime, 0, 0);
    }


    void Engine()
    {

        //Steer Limit.
        SteerAngle = Mathf.Lerp(defsteerAngle, highSpeedSteerAngle, (Speed / highSpeedSteerAngleAtSpeed));
        FrontWheelCollider.steerAngle = SteerAngle * steerInput;

        //Engine RPM.
        EngineRPM = Mathf.Clamp((((Mathf.Abs((FrontWheelCollider.rpm + RearWheelCollider.rpm)) * gearShiftRate) + MinEngineRPM)) / (currentGear + 1), MinEngineRPM, MaxEngineRPM);

        // Applying Motor Torque.
        if (Speed > maxSpeed)
        {
            RearWheelCollider.motorTorque = 0;
        }
        else if (!reversing)
        {
            RearWheelCollider.motorTorque = EngineTorque * Mathf.Clamp(motorInput, 0f, 1f);
        }

        if (reversing)
        {
            if (Speed < 10)
            {
                RearWheelCollider.motorTorque = (EngineTorque * motorInput) / 5f;
            }
            else
            {
                RearWheelCollider.motorTorque = 0;
            }
        }

    }

    public void Braking()
    {

        // Deceleration.
        if (Mathf.Abs(motorInput) <= .05f)
        {
            brakingNow = false;
            FrontWheelCollider.brakeTorque = (Brake) / 25f;
            RearWheelCollider.brakeTorque = (Brake) / 25f;
        }
        else if (motorInput < 0 && !reversing)
        {
            brakingNow = true;
            FrontWheelCollider.brakeTorque = (Brake) * (Mathf.Abs(motorInput) / 5f);
            RearWheelCollider.brakeTorque = (Brake) * (Mathf.Abs(motorInput));
        }
        else
        {
            brakingNow = false;
            FrontWheelCollider.brakeTorque = 0;
            RearWheelCollider.brakeTorque = 0;
        }

    }

    void WheelAlign()
    {

        RaycastHit hit;
        WheelHit CorrespondingGroundHit;
        float extension_F;
        float extension_R;

        Vector3 ColliderCenterPointFL = FrontWheelCollider.transform.TransformPoint(FrontWheelCollider.center);
        FrontWheelCollider.GetGroundHit(out CorrespondingGroundHit);

        if (Physics.Raycast(ColliderCenterPointFL, -FrontWheelCollider.transform.up, out hit, (FrontWheelCollider.suspensionDistance + FrontWheelCollider.radius) * transform.localScale.y))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Bike"))
            {
                FrontWheelTransform.transform.position = hit.point + (FrontWheelCollider.transform.up * FrontWheelCollider.radius) * transform.localScale.y;
                if (Fender)
                    Fender.transform.position = hit.point + (FrontWheelCollider.transform.up * (FrontWheelCollider.radius + FrontWheelCollider.suspensionDistance)) * transform.localScale.y;
                extension_F = (-FrontWheelCollider.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - FrontWheelCollider.radius) / FrontWheelCollider.suspensionDistance;
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + FrontWheelCollider.transform.up * (CorrespondingGroundHit.force / 8000), extension_F <= 0.0f ? Color.magenta : Color.white);
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - FrontWheelCollider.transform.forward * CorrespondingGroundHit.forwardSlip, Color.green);
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - FrontWheelCollider.transform.right * CorrespondingGroundHit.sidewaysSlip, Color.red);
            }
        }
        else
        {
            FrontWheelTransform.transform.position = ColliderCenterPointFL - (FrontWheelCollider.transform.up * FrontWheelCollider.suspensionDistance) * transform.localScale.y;
        }
        RotationValue1 += FrontWheelCollider.rpm * (6) * Time.deltaTime;
        FrontWheelTransform.transform.rotation = FrontWheelCollider.transform.rotation * Quaternion.Euler(RotationValue1, FrontWheelCollider.steerAngle, FrontWheelCollider.transform.rotation.z);

        Vector3 ColliderCenterPointRL = RearWheelCollider.transform.TransformPoint(RearWheelCollider.center);
        RearWheelCollider.GetGroundHit(out CorrespondingGroundHit);

        if (Physics.Raycast(ColliderCenterPointRL, -RearWheelCollider.transform.up, out hit, (RearWheelCollider.suspensionDistance + RearWheelCollider.radius) * transform.localScale.y))
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Bike"))
            {
                RearWheelTransform.transform.position = hit.point + (RearWheelCollider.transform.up * RearWheelCollider.radius) * transform.localScale.y;
                extension_R = (-RearWheelCollider.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - RearWheelCollider.radius) / RearWheelCollider.suspensionDistance;
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + RearWheelCollider.transform.up * (CorrespondingGroundHit.force / 8000), extension_R <= 0.0f ? Color.magenta : Color.white);
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - RearWheelCollider.transform.forward * CorrespondingGroundHit.forwardSlip, Color.green);
                Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - RearWheelCollider.transform.right * CorrespondingGroundHit.sidewaysSlip, Color.red);
            }
        }
        else
        {
            RearWheelTransform.transform.position = ColliderCenterPointRL - (RearWheelCollider.transform.up * RearWheelCollider.suspensionDistance) * transform.localScale.y;
        }
        RotationValue2 += RearWheelCollider.rpm * (6) * Time.deltaTime;
        RearWheelTransform.transform.rotation = RearWheelCollider.transform.rotation * Quaternion.Euler(RotationValue2, RearWheelCollider.steerAngle, RearWheelCollider.transform.rotation.z);

        //Steering Wheel and Fender transforms
        if (SteeringHandlebar)
            SteeringHandlebar.transform.rotation = FrontWheelCollider.transform.rotation * Quaternion.Euler(0, FrontWheelCollider.steerAngle, FrontWheelCollider.transform.rotation.z);
        if (Fender)
            Fender.rotation = FrontWheelCollider.transform.rotation * Quaternion.Euler(0, FrontWheelCollider.steerAngle, FrontWheelCollider.transform.rotation.z);

    }

    public void ShiftGears()
    {

        if (currentGear < _totalGears && !changingGear)
        {
            if (EngineRPM > (MaxEngineRPM - 500) && RearWheelCollider.rpm >= 0)
            {
                StartCoroutine("ChangingGear", currentGear + 1);
            }
        }

        if (currentGear > 0)
        {
            if (EngineRPM < MinEngineRPM + 500 && !changingGear)
            {

                for (int i = 0; i < gearSpeed.Length; i++)
                {
                    if (Speed > gearSpeed[i])
                        StartCoroutine("ChangingGear", i);
                }

            }
        }

    }

    IEnumerator ChangingGear(int gear)
    {

        changingGear = true;

        yield return new WaitForSeconds(.5f);
        changingGear = false;
        currentGear = gear;

    }

    void Lean()
    {

        verticalLean = Mathf.Clamp(Mathf.Lerp(verticalLean, transform.InverseTransformDirection(rigid.angularVelocity).x * chassisVerticalLean, Time.deltaTime * 5f), -10.0f, 10.0f);

        WheelHit CorrespondingGroundHit;
        FrontWheelCollider.GetGroundHit(out CorrespondingGroundHit);

        float normalizedLeanAngle = Mathf.Clamp(CorrespondingGroundHit.sidewaysSlip, -1f, 1f);

        if (transform.InverseTransformDirection(rigid.velocity).z > 0f)
            normalizedLeanAngle = -1;
        else
            normalizedLeanAngle = 1;

        horizontalLean = Mathf.Clamp(Mathf.Lerp(horizontalLean, (transform.InverseTransformDirection(rigid.angularVelocity).y * normalizedLeanAngle) * chassisHorizontalLean, Time.deltaTime * 3f), -50.0f, 50.0f);

        Quaternion target = Quaternion.Euler(verticalLean, chassis.transform.localRotation.y + (rigid.angularVelocity.z), horizontalLean);
        chassis.transform.localRotation = target;

        rigid.centerOfMass = new Vector3((COM.localPosition.x) * transform.localScale.x, (COM.localPosition.y) * transform.localScale.y, (COM.localPosition.z) * transform.localScale.z);

    }

    void OnCollisionEnter(Collision collision)
    {

        crashed = true;

    }

}