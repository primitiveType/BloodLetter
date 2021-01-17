using UnityEngine;

public class SpawnOnAwake : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabToSpawn;

    void Awake()
    {
        Instantiate(m_PrefabToSpawn, transform);
    }

}
