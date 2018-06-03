using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{

    public void PlaySound(string soundName)
    {
        AkSoundEngine.PostEvent(soundName,gameObject);
    }
}
