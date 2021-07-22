using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : SliderManager
{
    public float mainVolume = 0;
    private void Update()
    {
        base.Update();
        mainVolume = slider.value;
        AkSoundEngine.SetRTPCValue("MainVolume", mainVolume);
    }
}
// working with wwise inspired from: https://www.youtube.com/watch?v=YUlVlx4hHkU&ab_channel=DanielKim