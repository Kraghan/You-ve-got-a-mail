using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnSpot : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;
    [SerializeField]
    Vector3 m_rotationRespawn;

    BoxCollider m_boxCollider;
    List<GameObject> m_objectsInSpawnArea;

	// Use this for initialization
	void Start ()
    {
        m_boxCollider = GetComponent<BoxCollider>();
        m_boxCollider.isTrigger = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        m_objectsInSpawnArea.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_objectsInSpawnArea.Remove(other.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(m_rotationRespawn) * Vector3.one);
    }

    public void Respawn()
    {
        for(int i = 0; i < m_objectsInSpawnArea.Count; ++i)
        {
            m_objectsInSpawnArea[i].SetActive(false);
        }

        m_player.transform.position = transform.position;

        m_player.transform.rotation = Quaternion.Euler(m_rotationRespawn); 

    }
}
