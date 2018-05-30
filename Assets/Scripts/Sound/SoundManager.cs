using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public void SetVolumeMusic(Slider slider)
    {
        AkSoundEngine.SetRTPCValue("YGM_MUSICVOLUME", slider.value);
    }

    public void SetVolumeSFX(Slider slider)
    {
        AkSoundEngine.SetRTPCValue("YGM_VFXVOLUME", slider.value);
    }
}
