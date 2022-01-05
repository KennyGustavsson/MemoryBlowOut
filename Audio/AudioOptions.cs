using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    [SerializeField] private Slider ambienceSlider = null;
    [SerializeField] private Slider masterSlider = null;
    [SerializeField] private Slider musicSlider = null;
    [SerializeField] private Slider effectSlider = null;

    private const string ambience = "Ambience_Volume";
    private const string master = "Master_Volume";
    private const string music = "Music_Volume";
    private const string sfx = "SFX_Volume";
    
    private int type = 1;
    
    private void Awake()
    {
        if (ambienceSlider)
            ambienceSlider.value = GetRTPCValue(ambience);

        if (masterSlider)
            masterSlider.value = GetRTPCValue(master);

        if (musicSlider)
            musicSlider.value = GetRTPCValue(music);

        if (effectSlider)
            effectSlider.value = GetRTPCValue(sfx);
    }

    public void SetAmbienceVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(ambience, volume);
    }
    
    public void SetMasterVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(master, volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(music, volume);
    }
    
    public void SetEffectVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue(sfx, volume);
    }

    private void OnDestroy()
    {
        AkSoundEngine.StopAll();
    }

    private float GetRTPCValue(string rtpcID)
    {
        AkSoundEngine.GetRTPCValue(rtpcID, gameObject, 0, out float value, ref type);
        return value;
    }
}
