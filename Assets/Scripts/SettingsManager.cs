using System.Net;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviourSingleton<SettingsManager>
{
    public Settings Settings { get; private set; }
    [SerializeField] public AudioMixerGroup MusicMixerGroup;
    [SerializeField] private AudioMixerGroup SfxMixerGroup;
    private readonly string MusicVolumeKey = "MusicVolume";
    private readonly string SfxVolumeKey = "SfxVolume";
    private readonly string ScreenShakeKey = "ScreenShake";
    private readonly string PaintballModeKey = "PaintballMode";
    private readonly string SensitivityKey = "Sensitivity";

    protected void Start()
    {
        Settings = new Settings();
        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            Settings.MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
        }

        if (PlayerPrefs.HasKey(SfxVolumeKey))
        {
            Settings.SfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey);
        }

        if (PlayerPrefs.HasKey(ScreenShakeKey))
        {
            Settings.ScreenShake = PlayerPrefs.GetFloat(ScreenShakeKey);
        }
        
        if (PlayerPrefs.HasKey(PaintballModeKey))
        {
            Settings.PaintballMode = PlayerPrefs.GetInt(PaintballModeKey) == 1;
        }
        
        if (PlayerPrefs.HasKey(SensitivityKey))
        {
            Settings.Sensitivity = PlayerPrefs.GetFloat(SensitivityKey);
        }


        SetMusicVolume(Settings.MusicVolume);
        SetSfxVolume(Settings.SfxVolume);
        SetScreenShake(Settings.ScreenShake);
        SetSensitivity(Settings.Sensitivity);
        SetPaintballMode(Settings.PaintballMode);
    }


    public void SetMusicVolume(float volume)
    {
        Settings.MusicVolume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();

        var logVolume = Mathf.Log10(volume) * 20;
        logVolume = Mathf.Clamp(logVolume, -80, 0);
        MusicMixerGroup.audioMixer.SetFloat("Volume", logVolume);
    }

    public void SetSfxVolume(float volume)
    {
        Settings.SfxVolume = volume;
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
        PlayerPrefs.Save();

        var logVolume = Mathf.Log10(volume) * 20;
        logVolume = Mathf.Clamp(logVolume, -80, 0);
        SfxMixerGroup.audioMixer.SetFloat("Volume", logVolume);
    }

    public void SetScreenShake(float shake)
    {
        Settings.ScreenShake = shake;
        PlayerPrefs.SetFloat(ScreenShakeKey, shake);
        PlayerPrefs.Save();
    }

    public void SetPaintballMode(bool arg0)
    {
        Settings.PaintballMode = arg0;
        PlayerPrefs.SetInt(PaintballModeKey, arg0 ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetSensitivity(float sensitivity)
    {
        Settings.Sensitivity = sensitivity;
        PlayerPrefs.SetFloat(SensitivityKey, sensitivity);
        PlayerPrefs.Save();
    }
}