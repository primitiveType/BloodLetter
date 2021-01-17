using UnityEngine;

[CreateAssetMenu(menuName = "Create LevelInfo", fileName = "LevelInfo", order = 0)]
public class LevelInfo : ScriptableObject
{
    [SerializeField] private string m_LevelKey;
    [SerializeField] private string m_LevelName;

    [SerializeField] private string m_LevelLocation;

    [SerializeField] [TextArea(15, 20)] private string m_LevelDescription;

    public string LevelKey => m_LevelKey;

    public string LevelName => m_LevelName;

    public string LevelLocation => m_LevelLocation;

    public string LevelDescription => m_LevelDescription;
}