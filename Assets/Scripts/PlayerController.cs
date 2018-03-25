using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private GameObject m_player;
    [SerializeField]
    private float m_speed;

    [SerializeField]
    private SteamVR_TrackedObject m_controllerLeft;
    [SerializeField]
    private SteamVR_TrackedObject m_controllerRight;
    [SerializeField]
    private GameObject m_newspaperPrefab;
    
    [Header("Newspaper canon")]
    [SerializeField]
    private float m_ballisticCurvePrecision = 0.5f;
    [SerializeField]
    private float m_minForce = 5;
    [SerializeField]
    private float m_maxForce = 20;
    [SerializeField]
    private float m_timeToReachMaxForce = 2;
    private float m_timeElapsedForce = 0;

    private bool m_throwNewspaper = false;

    private GameObject m_newspaperInHand;

    private GameObject m_newspaperPool;

    void Start()
    {
        m_player = transform.gameObject;
        m_newspaperPool = new GameObject("Newspaper pool");
    }

	// Update is called once per frame
	void Update ()
    {
        SpeedManager();

        NewspaperManager();

        m_player.transform.position += new Vector3(1, 0, 0) * m_speed * Time.deltaTime;
	}

    void SpeedManager()
    {
        if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerLeft.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.y > 0.7f)
            {
                m_speed += 0.1f;
            }

            else if (touchpad.y < -0.7f)
            {
                m_speed -= 0.1f;
            }
        }
        else if (SteamVR_Controller.Input((int)m_controllerRight.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = (SteamVR_Controller.Input((int)m_controllerRight.index).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.y > 0.7f)
            {
                m_speed += 0.1f;
            }

            else if (touchpad.y < -0.7f)
            {
                m_speed -= 0.1f;
            }
        }
    }

    void NewspaperManager()
    {
        // Grab
        if (SteamVR_Controller.Input((int)m_controllerLeft.index).GetPress(SteamVR_Controller.ButtonMask.Trigger)
            && !m_newspaperInHand)
        {
            m_newspaperInHand = Instantiate(m_newspaperPrefab);
            m_newspaperInHand.transform.position = m_controllerLeft.transform.position;
            m_newspaperInHand.transform.rotation = m_controllerLeft.transform.rotation;
            FixedJoint joint = m_controllerLeft.gameObject.AddComponent<FixedJoint>();
            joint.breakForce = 20000;
            joint.breakTorque = 20000;
            joint.connectedBody = m_newspaperInHand.GetComponent<Rigidbody>();
        }
        // Release
        else if(!SteamVR_Controller.Input((int)m_controllerLeft.index).GetPress(SteamVR_Controller.ButtonMask.Trigger)
            && m_newspaperInHand)
        {
            FixedJoint joint = m_controllerLeft.gameObject.GetComponent<FixedJoint>();
            if (joint)
            {
                joint.connectedBody = null;
                Destroy(joint);

                m_newspaperInHand.GetComponent<Rigidbody>().velocity = SteamVR_Controller.Input((int)m_controllerLeft.index).velocity;
                m_newspaperInHand.GetComponent<Rigidbody>().angularVelocity = SteamVR_Controller.Input((int)m_controllerLeft.index).angularVelocity;
            }
            m_newspaperInHand.transform.parent = m_newspaperPool.transform;
            m_newspaperInHand = null;
        }

        if (SteamVR_Controller.Input((int)m_controllerRight.index).GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            /*m_newspaperInHand = Instantiate(m_newspaperPrefab);
            m_newspaperInHand.transform.position = m_controllerRight.transform.position;
            m_newspaperInHand.transform.rotation = m_controllerRight.transform.rotation;
            FixedJoint joint = m_controllerRight.gameObject.AddComponent<FixedJoint>();
            joint.breakForce = 20000;
            joint.breakTorque = 20000;
            joint.connectedBody = m_newspaperInHand.GetComponent<Rigidbody>();*/
            DrawBallisticCurve();
            m_throwNewspaper = true;
            m_timeElapsedForce += Time.deltaTime;
        }
        else if(m_throwNewspaper && !SteamVR_Controller.Input((int)m_controllerRight.index).GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            m_controllerRight.GetComponent<LineRenderer>().enabled = false;
            ThrowNewspaper();
            m_throwNewspaper = false;
            m_timeElapsedForce = 0;
        }
    }

    void DrawBallisticCurve()
    {
        List<Vector3> curvePoints = new List<Vector3>();
        float time = 0;
        Vector3 origin = m_controllerRight.transform.position;

        Transform[] childrenTransform = m_controllerRight.GetComponentsInChildren<Transform>();
        Vector3 direction = new Vector3();
        for(int i = 0; i < childrenTransform.Length; ++i)
        {
            if (childrenTransform[i].gameObject.name == "Pointer")
            {
                direction = childrenTransform[i].position - origin;
            }
        }
        direction.Normalize();

        float force = Mathf.Lerp(m_minForce, m_maxForce, Mathf.Clamp(m_timeElapsedForce / m_timeToReachMaxForce,0,1));

        Vector3 position = CalculatePositionAtTime(time, origin, direction, force);
        while (position.y > 0 && curvePoints.Count < 50)
        {
            curvePoints.Add(position);
            time += m_ballisticCurvePrecision;
            position = CalculatePositionAtTime(time, origin, direction, force);
        }
        curvePoints.Add(position);

        LineRenderer renderer = m_controllerRight.GetComponent<LineRenderer>();
        renderer.positionCount = curvePoints.Count;
        renderer.SetPositions(curvePoints.ToArray());
        renderer.enabled = true;
    }

    void ThrowNewspaper()
    {
        Vector3 origin = m_controllerRight.transform.position;
        Transform[] childrenTransform = m_controllerRight.GetComponentsInChildren<Transform>();
        Vector3 direction = new Vector3();
        float force = Mathf.Lerp(m_minForce, m_maxForce, Mathf.Clamp(m_timeElapsedForce / m_timeToReachMaxForce, 0, 1));

        for (int i = 0; i < childrenTransform.Length; ++i)
        {
            if (childrenTransform[i].gameObject.name == "Pointer")
            {
                direction = childrenTransform[i].position - origin;
            }
        }
        direction.Normalize();

        GameObject newspaper = Instantiate(m_newspaperPrefab);
        newspaper.transform.position = m_controllerRight.transform.position;
        newspaper.transform.rotation = m_controllerRight.transform.rotation;
        newspaper.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
    }

    Vector3 CalculatePositionAtTime(float time, Vector3 origin, Vector3 directionNormalized, float speed)
    {
        Vector3 vec = origin;

        vec += (directionNormalized * speed) * time;

        vec.y += 0.5f * Physics.gravity.y * time * time;

        return vec;
    }

}
