using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SoundPlayer : MonoBehaviour
{

    public void PlaySound(string soundName)
    {
        AkSoundEngine.PostEvent(soundName,gameObject);
    }
}
