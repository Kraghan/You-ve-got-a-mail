using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    public uint m_score;
    public float m_radius;
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Target : MonoBehaviour {

    SpriteRenderer m_renderer;
    BoxCollider m_collider;

    [SerializeField]
    bool m_triggered = false;

    [SerializeField]
    Score[] m_scoreZone;

    [SerializeField]
    GameObject m_ballPool;
    
    public static uint s_score = 0;
    public static uint s_target = 0;
    public static float s_scoreDistance = 0;


    // Use this for initialization
    void Start ()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (m_triggered)
            m_renderer.color = new Color(0, 0, 0, 0);
        else
            m_renderer.color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_triggered && other.CompareTag("Mail"))
        {
            s_target++;
            m_triggered = true;
            Vector3 point = m_collider.ClosestPoint(other.transform.position);

            float distance = Vector3.Distance(transform.position, point);
            s_scoreDistance += distance;

            for(int i = 0; i < m_scoreZone.Length; ++i)
            {
                if (m_scoreZone[i].m_radius >= distance)
                {
                    s_score += m_scoreZone[i].m_score;
                    break;
                }
            }

            Debug.Log("Score : " + s_score + " Target : " + s_target + " Ratio : " + (float)s_score / (float)s_target);
            Debug.Log("Score distance : " + s_scoreDistance + " Ratio : " + (float)s_scoreDistance / (float)s_target);
            float projectiles = m_ballPool.transform.childCount;
            Debug.Log("Ball shooted : " + projectiles + " Ratio score : " + (float)s_score / projectiles + " Ratio distance : " + (float)s_scoreDistance / projectiles);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for(uint i = 0; i < m_scoreZone.Length; ++i)
        {
            Gizmos.DrawWireSphere(transform.position, m_scoreZone[i].m_radius);
        }
    }

}
