using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameConstants : MonoBehaviourSingleton<GameConstants>
{
    private static Dictionary<string, EnemyData> CachedEnemyData = new Dictionary<string, EnemyData>();
    private static Dictionary<string, ProjectileData> CachedProjectileData = new Dictionary<string, ProjectileData>();
    private static Dictionary<string, PlayerData> CachedPlayerData = new Dictionary<string, PlayerData>();

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
            Debug.LogException(new Exception($"Projectile data not found at path : {path}"));
        }

        string json = File.ReadAllText(path);
        var newData = JsonUtility.FromJson<ProjectileData>(json);
        CachedProjectileData[path] = newData;

        return newData;
    }

    public static string GetPlayerDataPath(string difficulty)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Game Data", "Player", difficulty);

        return path;
    }

    public static PlayerData GetPlayerData(string difficulty)
    {
        var path = GetPlayerDataPath(difficulty);
        if (Application.isPlaying)
        {
            if (CachedPlayerData.TryGetValue(path, out PlayerData data))
            {
                return data;
            }
        }

        if (!File.Exists(path))
        {
            Debug.LogException(new Exception($"Player data not found at path : {path}"));
        }

        string json = File.ReadAllText(path);
        var newData = JsonUtility.FromJson<PlayerData>(json);
        CachedPlayerData[path] = newData;

        return newData;
    }
}