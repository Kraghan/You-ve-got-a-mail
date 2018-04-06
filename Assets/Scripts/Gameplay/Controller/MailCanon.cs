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
    private float m_timeToReachMaxForce = 2;
    private float m_timeElapsedForce = 0;

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
            m_timeElapsedForce += Time.deltaTime;
        }
        else if (m_throwNewspaper && !Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            GetComponent<LineRenderer>().enabled = false;
            ThrowNewspaper();
            m_throwNewspaper = false;
            m_timeElapsedForce = 0;
        }
    }

    void DrawBallisticCurve()
    {
        List<Vector3> curvePoints = new List<Vector3>();
        float time = 0;
        Vector3 origin = transform.position;

        Transform[] childrenTransform = GetComponentsInChildren<Transform>();
        Vector3 direction = new Vector3();
        for (int i = 0; i < childrenTransform.Length; ++i)
        {
            if (childrenTransform[i].gameObject.name == "Pointer")
            {
                direction = childrenTransform[i].position - origin;
            }
        }
        direction.Normalize();

        float force = Mathf.Lerp(m_minForce, m_maxForce, Mathf.Clamp(m_timeElapsedForce / m_timeToReachMaxForce, 0, 1));

        Vector3 position = CalculatePositionAtTime(time, origin, direction, force);
        while (position.y > 0 && curvePoints.Count < 50)
        {
            curvePoints.Add(position);
            time += m_ballisticCurvePrecision;
            position = CalculatePositionAtTime(time, origin, direction, force);
        }
        curvePoints.Add(position);

        LineRenderer renderer = GetComponent<LineRenderer>();
        renderer.positionCount = curvePoints.Count;
        renderer.SetPositions(curvePoints.ToArray());
        renderer.enabled = true;
    }

    void ThrowNewspaper()
    {
        Vector3 origin = transform.position;
        Transform[] childrenTransform = GetComponentsInChildren<Transform>();
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
        newspaper.transform.position = transform.position;
        newspaper.transform.rotation = transform.rotation;
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
