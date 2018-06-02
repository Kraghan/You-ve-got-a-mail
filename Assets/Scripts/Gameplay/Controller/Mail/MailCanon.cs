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
    private ProbabilityOfAppearenceOfItem[] m_aMailsPrefabs;
    [SerializeField]
    private Rigidbody m_bikeBody;
    Animator m_model;

    [SerializeField]
    Timer m_timeToReachMaxForce = new Timer();
    [SerializeField]
    float m_minForce = 2;
    [SerializeField]
    float m_maxForce = 10;

    [SerializeField]
    Transform m_startPointLaser;

    private bool m_throwNewspaper = false;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        SetAnimator();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!m_model.GetBool("Grab") && !m_model.GetBool("Pointing") 
            && Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            DrawBallisticCurve();
            m_throwNewspaper = true;
            m_model.SetBool("Gun", true);
            m_timeToReachMaxForce.UpdateTimer();
        }
        else if (m_throwNewspaper && !Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            GetComponent<LineRenderer>().enabled = false;
            ThrowNewspaper();
            m_throwNewspaper = false;
            m_model.SetBool("Gun", false);
            m_timeToReachMaxForce.Restart();
        }
    }
    
    void ThrowNewspaper()
    {
        Vector3 direction = transform.forward;
        direction.Normalize();

        GameObject newspaper = Instantiate(PickMail().gameObject, m_startPointLaser.position, transform.rotation) as GameObject;
        Rigidbody body = newspaper.GetComponent<Rigidbody>();
        body.angularVelocity = m_bikeBody.angularVelocity;
        body.velocity = m_bikeBody.velocity;
        body.AddForce(direction * Mathf.Lerp(m_minForce, m_maxForce, Mathf.Clamp(m_timeToReachMaxForce.GetRatio(), 0, 1)), ForceMode.Impulse);
        AkSoundEngine.PostEvent("YGM_Gunshot", gameObject);
    }

    public void SetForce(float minForce, float maxForce)
    {
        m_minForce = minForce;
        m_maxForce = maxForce;
    }

    public void SetTimer(Timer timer)
    {
        m_timeToReachMaxForce.Start(timer.GetTimeToReach());
    }

    public void SetLaserOrigin(Transform origin)
    {
        m_startPointLaser = origin;
    }

    public void SetObjectToSend(ProbabilityOfAppearenceOfItem[] items)
    {
        m_aMailsPrefabs = items;
    }

    public void SetBikeRigidbody(Rigidbody body)
    {
        m_bikeBody = body;
    }

    private Mail PickMail()
    {

        float maxPercentage = 0;
        foreach (ProbabilityOfAppearenceOfItem proba in m_aMailsPrefabs)
        {
            maxPercentage += proba.m_probability;
        }

        float value = Random.Range(0, maxPercentage);
        float storage = 0;
        foreach (ProbabilityOfAppearenceOfItem proba in m_aMailsPrefabs)
        {
            storage += proba.m_probability;
            if (value < storage)
                return proba.m_item;
        }

        return null;
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

        Vector3 origin = m_startPointLaser.position;
        Vector3 position = CalculatePositionAtTime(time, origin, direction, force);
        while (position.y > 0 && curvePoints.Count < 50)
        {
            curvePoints.Add(position);
            time += 0.5f;
            position = CalculatePositionAtTime(time, origin, direction, force);
        }
        curvePoints.Add(position);

        renderer.positionCount = curvePoints.Count;
        renderer.SetPositions(curvePoints.ToArray());
        renderer.enabled = true;
    }

    Vector3 CalculatePositionAtTime(float time, Vector3 origin, Vector3 directionNormalized, float speed)
    {
        Vector3 vec = origin;

        vec += (directionNormalized * speed) * time;

        vec.y += 0.5f * Physics.gravity.y * time * time;

        return vec;
    }
    
    public void SetAnimator()
    {
        m_model = GetComponentInChildren<Animator>();
    }
}
