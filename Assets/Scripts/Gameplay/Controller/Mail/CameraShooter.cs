using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShooter : MonoBehaviour {

    [SerializeField]
    private SteamVR_TrackedObject m_trackController;

    private SteamVR_TrackedObject trackedObj;

    [SerializeField]
    private GameObject m_newspaperPrefab;
    
    [SerializeField]
    private float m_force = 20;

    private bool m_throwNewspaper = false;

    [SerializeField]
    private Rigidbody m_bikeBody;

    [SerializeField]
    private GameObject m_pool;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Controller.Input((int)m_trackController.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            m_throwNewspaper = true;
        }
        else if (m_throwNewspaper && !SteamVR_Controller.Input((int)m_trackController.index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            ThrowNewspaper();
            m_throwNewspaper = false;
        }
    }
    
    void ThrowNewspaper()
    {
        Vector3 direction = transform.forward;
        direction.Normalize();

        GameObject newspaper = Instantiate(m_newspaperPrefab);
        newspaper.transform.position = transform.position;
        newspaper.transform.rotation = transform.rotation;
        Rigidbody body = newspaper.GetComponent<Rigidbody>();
        body.angularVelocity = m_bikeBody.angularVelocity;
        body.velocity = m_bikeBody.velocity;
        body.AddForce(direction * m_force, ForceMode.Impulse);
        newspaper.transform.parent = m_pool.transform;
    }
}
