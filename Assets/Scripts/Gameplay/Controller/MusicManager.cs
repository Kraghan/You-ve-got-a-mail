using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {

	public void SetSoundVolume(Slider slider)
    {
        AkSoundEngine.SetRTPCValue("YGM_VFXVOLUME", slider.value);
    }

    public void SetMusicVolume(Slider slider)
    {
        AkSoundEngine.SetRTPCValue("YGM_MUSICVOLUME", slider.value);
    }
}
