using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviourSingleton<SettingsManager>
{
    private class Settings
    {
        public float MusicVolume { get; set; }
        public float SfxVolume { get; set; }
    }

    private Settings settings; 
    [SerializeField]public AudioMixerGroup MusicMixerGroup;
    [SerializeField]private AudioMixerGroup SfxMixerGroup;

    protected override void Awake()
    {
        settings = new Settings();
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, -80, 0);
        settings.MusicVolume = volume;
        MusicMixerGroup.audioMixer.SetFloat("Volume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp(volume, -80, 0);
        settings.SfxVolume = volume;
        SfxMixerGroup.audioMixer.SetFloat("Volume", volume);
    }
}