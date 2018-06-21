using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SafePylone : MonoBehaviour
{
    [SerializeField]
    RespawnSpot m_spot;

    [SerializeField]
    GameObject m_activeObjects;

    MeshRenderer[] m_aRenderer;

    [SerializeField]
    Color m_emissionColor = new Color(0, 1, 1);

	public bool firstpylone = false;

    public void Start()
    {
        m_aRenderer = GetComponentsInChildren<MeshRenderer>();
		if (!firstpylone)
        	SetInactive();
    }

    public void Respawn()
    {
        m_spot.Respawn();
    }

    private void OnTriggerEnter(Collider other)
    {
		if ((other.CompareTag("Player")) && (other.GetComponent<CrashDetection>().timer > other.GetComponent<CrashDetection>().pause))
        {
            other.GetComponent<CrashDetection>().SetRespawnPylone(this);
            m_spot.SetLockedRotation();
			other.GetComponent<CrashDetection>().timer = 0;
        }
    }

    public void SetActive()
    {
        foreach(MeshRenderer renderer in m_aRenderer)
            renderer.material.SetColor("_EmissionColor", m_emissionColor);
        m_activeObjects.SetActive(true);
    }

    public void SetInactive()
    {
        m_activeObjects.SetActive(false);
    }
}
