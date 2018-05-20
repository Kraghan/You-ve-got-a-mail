using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SafePylone : MonoBehaviour
{
    [SerializeField]
    RespawnSpot m_spot;

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
}
