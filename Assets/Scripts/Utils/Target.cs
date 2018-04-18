using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour {

    SpriteRenderer m_renderer;
    [SerializeField]
    bool m_triggered = false;

	// Use this for initialization
	void Start ()
    {
        m_renderer = GetComponent<SpriteRenderer>();
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
        if (other.CompareTag("Mail"))
            m_triggered = true;
    }

}
