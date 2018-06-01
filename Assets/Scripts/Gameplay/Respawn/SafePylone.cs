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

    public void Start()
    {
        m_aRenderer = GetComponentsInChildren<MeshRenderer>();
        SetInactive();
    }

    public void Respawn()
    {
        m_spot.Respawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<CrashDetection>().SetRespawnPylone(this);
            m_spot.SetLockedRotation();
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
