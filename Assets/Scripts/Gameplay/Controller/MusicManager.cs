using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {

	public void SetSoundVolume(Slider slider)
    {
        AkSoundEngine.SetRTPCValue("", slider.value);
    }

    public void SetMusicVolume(Slider slider)
    {
        AkSoundEngine.SetRTPCValue("", slider.value);
    }
}
