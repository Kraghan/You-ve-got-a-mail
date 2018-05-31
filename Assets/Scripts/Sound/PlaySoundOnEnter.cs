using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlaySoundOnEnter : MonoBehaviour {

    [SerializeField]
    string m_wwiseEventNameStart;
    [SerializeField]
    string m_wwiseEventNameStop;
    [SerializeField]
    Rigidbody m_source;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            AkSoundEngine.PostEvent(m_wwiseEventNameStart, m_source.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && m_wwiseEventNameStop != "")
        {
            AkSoundEngine.PostEvent(m_wwiseEventNameStart, m_source.gameObject);
        }
    }
}
