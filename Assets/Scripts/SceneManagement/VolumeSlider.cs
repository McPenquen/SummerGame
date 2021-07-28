using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : SliderManager
{
    public float mainVolume = 0;
    private float returnVal = 0;
    private void Start()
    {
        base.Start();
        // Load the slider value to be the one of the Master_Volume RTPC
        int valueType = (int)AkQueryRTPCValue.RTPCValue_Global; 
        AkSoundEngine.GetRTPCValue("Master_Volume", null, 0, out returnVal, ref valueType);
        slider.value = returnVal;
    }
    // RTPC-getter inspired from: https://www.audiokinetic.com/qa/5592/need-understanding-getrtpcvalue-updating-rtpcvalue-unity
    private void Update()
    {
        base.Update();
        mainVolume = slider.value;

        // Update all RTCP values to the sliders value
        AkSoundEngine.SetRTPCValue("Music_Volume", mainVolume);
        AkSoundEngine.SetRTPCValue("SFX_Volume", mainVolume);
        AkSoundEngine.SetRTPCValue("Master_Volume", mainVolume);        
    }   
}
// Qorking with wwise inspired from: https://www.youtube.com/watch?v=YUlVlx4hHkU&ab_channel=DanielKim