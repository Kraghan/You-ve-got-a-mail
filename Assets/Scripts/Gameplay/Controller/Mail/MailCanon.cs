using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MailCanon : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    [SerializeField]
    private GameObject m_newspaperPrefab;
    [SerializeField]
    private float m_ballisticCurvePrecision = 0.5f;
    [SerializeField]
    private float m_minForce = 5;
    [SerializeField]
    private float m_maxForce = 20;
    [SerializeField]
    private Timer m_timeToReachMaxForce;
    [SerializeField]
    private Rigidbody m_bikeBody;
    [SerializeField]
    private GameObject m_pool;

    private bool m_throwNewspaper = false;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            DrawBallisticCurve();
            m_throwNewspaper = true;
            m_timeToReachMaxForce.UpdateTimer();
        }
        else if (m_throwNewspaper && !Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            GetComponent<LineRenderer>().enabled = false;
            ThrowNewspaper();
            m_throwNewspaper = false;
            m_timeToReachMaxForce.Restart();
        }
    }

    void DrawBallisticCurve()
    {
        LineRenderer renderer = GetComponent<LineRenderer>();
        if (!renderer)
            return;

        List<Vector3> curvePoints = new List<Vector3>();
        float time = 0;
        Vector3 direction = trackedObj.transform.forward;
        direction.Normalize();

        float force = Mathf.Lerp(m_minForce, m_maxForce, Mathf.Clamp(m_timeToReachMaxForce.GetRatio(), 0, 1));

        Vector3 origin = trackedObj.transform.position;
        Vector3 position = CalculatePositionAtTime(time, origin, direction, force);
        while (position.y > 0 && curvePoints.Count < 50)
        {
            curvePoints.Add(position);
            time += m_ballisticCurvePrecision;
            position = CalculatePositionAtTime(time, origin, direction, force);
        }
        curvePoints.Add(position);
        
        renderer.positionCount = curvePoints.Count;
        renderer.SetPositions(curvePoints.ToArray());
        renderer.enabled = true;
    }

    void ThrowNewspaper()
    {
        float force = Mathf.Lerp(m_minForce, m_maxForce, Mathf.Clamp(m_timeToReachMaxForce.GetRatio(), 0, 1));

        Vector3 direction = transform.forward;
        direction.Normalize();

        GameObject newspaper = Instantiate(m_newspaperPrefab);
        newspaper.transform.position = transform.position;
        newspaper.transform.rotation = transform.rotation;
        Rigidbody body = newspaper.GetComponent<Rigidbody>();
        body.angularVelocity = m_bikeBody.angularVelocity;
        body.velocity = m_bikeBody.velocity;
        body.AddForce(direction * force, ForceMode.Impulse);
        newspaper.transform.parent = m_pool.transform;
    }

    Vector3 CalculatePositionAtTime(float time, Vector3 origin, Vector3 directionNormalized, float speed)
    {
        Vector3 vec = origin;

        vec += (directionNormalized * speed) * time;

        vec.y += 0.5f * Physics.gravity.y * time * time;

        return vec;
    }
}
