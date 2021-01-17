using System.Collections.Generic;
using UnityEngine;

public class LevelInfoDatabase : MonoBehaviourSingleton<LevelInfoDatabase>
{
    [SerializeField] private List<LevelInfo> LevelInfos;
    private readonly Dictionary<string, LevelInfo> m_levelInfosDictionary = new Dictionary<string, LevelInfo>();

    private Dictionary<string, LevelInfo> LevelInfosDictionary => m_levelInfosDictionary;

    private void Start()
    {
        foreach (LevelInfo info in LevelInfos)
        {
            LevelInfosDictionary.Add(info.LevelKey, info);
        }
    }

    public LevelInfo GetLevelInfo(string levelName)
    {
        if (!LevelInfosDictionary.ContainsKey(levelName))
        {
            return null;
        }
        return LevelInfosDictionary[levelName];
    }
}