using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Audio;

public class GameConstants : MonoBehaviourSingleton<GameConstants>
{
    private static Dictionary<string, EnemyData> CachedEnemyData = new Dictionary<string, EnemyData>();
    private static Dictionary<string, ProjectileData> CachedProjectileData = new Dictionary<string, ProjectileData>();

    public static string GetEnemyDataPath(string enemyName)
    {
        string dataPath = Path.Combine(Application.streamingAssetsPath, "Game Data", "Enemies");
        string path = Path.Combine(dataPath, enemyName);
        return path;
    }
    public static string GetProjectileDataPath(string projectileName)
    {
        string dataPath = Path.Combine(Application.streamingAssetsPath, "Game Data", "Attacks");
        string path = Path.Combine(dataPath, projectileName);
        return path;
    }
    public static EnemyData GetEnemyDataByName(string name, string difficulty)
    {
        string path = GetEnemyDataPath(name);
        return GetEnemyData(path);
    }
    
    public static ProjectileData GetProjectileDataByName(string name)
    {
        string path = GetProjectileDataPath(name);
        return GetProjectileData(path);
    }

    public static EnemyData GetEnemyData(string path)
    {
        if (Application.isPlaying)
        {
            if (CachedEnemyData.TryGetValue(path, out EnemyData data))
            {
                return data;
            }
        }

        if (!File.Exists(path))
        {
            Debug.LogException(new Exception($"Enemy data not found at path : {path}"));
        }

        string json = File.ReadAllText(path);
        var newData = JsonUtility.FromJson<EnemyData>(json);
        CachedEnemyData[path] = newData;

        return newData;
    }
    
    public static ProjectileData GetProjectileData(string path)
    {
        if (Application.isPlaying)
        {
            if (CachedProjectileData.TryGetValue(path, out ProjectileData data))
            {
                return data;
            }
        }

        if (!File.Exists(path))
        {
            Debug.LogException(new Exception($"Enemy data not found at path : {path}"));
        }

        string json = File.ReadAllText(path);
        var newData = JsonUtility.FromJson<ProjectileData>(json);
        CachedProjectileData[path] = newData;

        return newData;
    }
}

public class SettingsManager : MonoBehaviourSingleton<SettingsManager>
{
    public Settings Settings { get; private set; }
    [SerializeField] public AudioMixerGroup MusicMixerGroup;
    [SerializeField] private AudioMixerGroup SfxMixerGroup;
    private readonly string MusicVolumeKey = "MusicVolume";
    private readonly string SfxVolumeKey = "SfxVolume";
    private readonly string ScreenShakeKey = "ScreenShake";

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

        SetMusicVolume(Settings.MusicVolume);
        SetSfxVolume(Settings.SfxVolume);
        SetScreenShake(Settings.ScreenShake);
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
}