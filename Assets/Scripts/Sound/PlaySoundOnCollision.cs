using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour {

    [SerializeField]
    string m_collideOnlyWithTag = "";
    [SerializeField]
    string m_wwiseEventName = "";
    bool m_isPlayingSound = false;

    PlaySoundOnCollision m_parent = null;

    private void Start()
    {
        if (GetComponent<Collider>() && !GetComponent<Collider>().isTrigger)
            return;

        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider c in colliders)
        {
            PlaySoundOnCollision sound = c.GetComponent<PlaySoundOnCollision>();
            if (!sound || sound.GetEventName() != m_wwiseEventName)
            {
                sound = c.gameObject.AddComponent<PlaySoundOnCollision>();
                sound.SetCollisionTag(m_collideOnlyWithTag);
                sound.SetEventName(m_wwiseEventName);
                sound.SetParent(this);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_collideOnlyWithTag == ""
            || collision.collider.CompareTag(m_collideOnlyWithTag))
        {
            PlayCollisionSound();
        }
    }

    void Terminate(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_EndOfEvent)
            m_isPlayingSound = false;
    }

    public string GetEventName()
    {
        return m_wwiseEventName;
    }

    public void SetEventName(string name)
    {
        m_wwiseEventName = name;
    }

    public void SetCollisionTag(string collisionTag)
    {
        m_collideOnlyWithTag = collisionTag;
    }

    public void SetParent(PlaySoundOnCollision parent)
    {
        m_parent = parent;
    }

    public void PlayCollisionSound()
    {
        if (m_isPlayingSound)
            return;
        if (m_parent == null)
        {
            AkSoundEngine.PostEvent(m_wwiseEventName, gameObject, (uint)AkCallbackType.AK_EndOfEvent, Terminate, null);
            m_isPlayingSound = true;
        }
        else
            m_parent.PlayCollisionSound();
    }
}
