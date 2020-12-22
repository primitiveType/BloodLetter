using UnityEngine;

public class LevelReferenceComponent : MonoBehaviour
{
    [SerializeField] private string m_LevelName;
    public string LevelName => m_LevelName;
}