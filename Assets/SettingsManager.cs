using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviourSingleton<SettingsManager>
{
    public Settings Settings { get; private set; } 
    [SerializeField]public AudioMixerGroup MusicMixerGroup;
    [SerializeField]private AudioMixerGroup SfxMixerGroup;

    protected override void Awake()
    {
        Settings = new Settings();
        Settings.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        Settings.SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        SetMusicVolume(Settings.MusicVolume);
        SetSfxVolume(Settings.SfxVolume);
    }
    

    public void SetMusicVolume(float volume)
    {
 
        Settings.MusicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        
        var logVolume = Mathf.Log10(volume) * 20;
        logVolume = Mathf.Clamp(logVolume, -80, 0);
        MusicMixerGroup.audioMixer.SetFloat("Volume", logVolume);
    }
    public void SetSfxVolume(float volume)
    {
    
        Settings.SfxVolume = volume;
        PlayerPrefs.SetFloat("SfxVolume", volume);
        PlayerPrefs.Save();
        
        var logVolume = Mathf.Log10(volume) * 20;
        logVolume = Mathf.Clamp(logVolume, -80, 0);
        SfxMixerGroup.audioMixer.SetFloat("Volume", logVolume);
    }
}