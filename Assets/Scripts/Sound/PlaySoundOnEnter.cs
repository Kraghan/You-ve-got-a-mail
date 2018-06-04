using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlaySoundOnEnter : MonoBehaviour {

    [SerializeField]
    string m_wwiseEventNameCity;
    [SerializeField]
    string m_wwiseEventNameTuto;
    [SerializeField]
    Rigidbody m_source;
    private string presentmusic = "";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if ((presentmusic == "") || (presentmusic == m_wwiseEventNameTuto))
                presentmusic = m_wwiseEventNameCity;
            else
                presentmusic = m_wwiseEventNameTuto;

            AkSoundEngine.PostEvent(presentmusic, m_source.gameObject);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && m_wwiseEventNameStop != "")
        {
            AkSoundEngine.PostEvent(m_wwiseEventNameStop, m_source.gameObject);
        }
    }*/
}
