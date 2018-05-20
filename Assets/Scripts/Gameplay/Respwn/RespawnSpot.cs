using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnSpot : MonoBehaviour
{
    GameObject m_player;
    [SerializeField]
    Vector3 m_rotationRespawn;
    [SerializeField]
    bool m_ignoreOrientation = false;

    BoxCollider m_boxCollider;
    List<GameObject> m_objectsInSpawnArea = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {

        m_player = GameObject.FindGameObjectWithTag("Player");
        m_boxCollider = GetComponent<BoxCollider>();
        m_boxCollider.isTrigger = true;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mover"))
            m_objectsInSpawnArea.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mover"))
            m_objectsInSpawnArea.Remove(other.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        Vector3 direction = m_rotationRespawn.normalized / 2;
        direction = transform.rotation * direction;
        Color color = Color.magenta;
        float arrowHeadLength = 0.25f;
        float arrowHeadAngle = 20;

        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);

        if (!m_ignoreOrientation)
        {
            Gizmos.DrawRay(pos, -direction);
            right = Quaternion.LookRotation(-direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            left = Quaternion.LookRotation(-direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos - direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos - direction, left * arrowHeadLength);
        }
    }

    public void Respawn()
    {
        for(int i = 0; i < m_objectsInSpawnArea.Count; ++i)
        {
            m_objectsInSpawnArea[i].SetActive(false);
        }

        m_player.transform.position = transform.position;

        Vector3 rotation = transform.rotation * m_rotationRespawn;

        float angle = Vector3.SignedAngle(m_player.transform.forward, rotation, Vector3.up);

        m_player.transform.rotation = Quaternion.LookRotation(rotation);
        
        if (!m_ignoreOrientation && angle < 0)
            m_player.transform.Rotate(Vector3.up, 180);
    }
}
