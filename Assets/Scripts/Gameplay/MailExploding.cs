using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailExploding : Mail
{
    [SerializeField]
    GameObject[] m_aLinkedObjects;

    [SerializeField]
    float m_forceToAddMin = 2;
    [SerializeField]
    float m_forceToAddMax = 5;

    protected override void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Mail"))
            return;
        if (!m_hasCollide)
        {
            Rigidbody bodyParent = GetComponent<Rigidbody>();
            
            foreach(GameObject go in m_aLinkedObjects)
            {
                Rigidbody body = go.gameObject.AddComponent<Rigidbody>();
                go.AddComponent<Mail>();
                go.transform.parent = null;
                body.velocity = bodyParent.velocity + new Vector3(Random.Range(m_forceToAddMin, m_forceToAddMax), Random.Range(m_forceToAddMin, m_forceToAddMax), Random.Range(m_forceToAddMin, m_forceToAddMax));
            }
        }
        base.OnCollisionEnter(collision);
        
    }
}
