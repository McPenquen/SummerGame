using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Types of volumes
enum VolumeType {
    SFXVolume,
    MusicVolume
}

public class VolumeSlider : SliderManager
{
    public float volume = 0;
    private float returnVal = 0;
    // The type of volume to control
    [SerializeField] private VolumeType chosenVolumeType = VolumeType.MusicVolume;

    private void Start()
    {
        base.Start();
        // Load the slider value based on the volume type
        int valueType = (int)AkQueryRTPCValue.RTPCValue_Global; 
        if (chosenVolumeType== VolumeType.SFXVolume)
        {
            AkSoundEngine.GetRTPCValue("SFX_Volume", null, 0, out returnVal, ref valueType);
        }
        else
        {
            AkSoundEngine.GetRTPCValue("Music_Volume", null, 0, out returnVal, ref valueType);
        }
        slider.value = returnVal;
    }
    // RTPC-getter inspired from: https://www.audiokinetic.com/qa/5592/need-understanding-getrtpcvalue-updating-rtpcvalue-unity
    private void Update()
    {
        base.Update();
        volume = slider.value;

        // Update the chosen RTCP value to the sliders value
        if (chosenVolumeType== VolumeType.SFXVolume)
        {
           AkSoundEngine.SetRTPCValue("SFX_Volume", volume);  
        }
        else
        {
            AkSoundEngine.SetRTPCValue("Music_Volume", volume); 
        }      
    }   
}
// Qorking with wwise inspired from: https://www.youtube.com/watch?v=YUlVlx4hHkU&ab_channel=DanielKim