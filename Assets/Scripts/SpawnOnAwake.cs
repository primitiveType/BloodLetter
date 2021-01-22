using UnityEngine;
using UnityEngine.AI;

public class SpawnOnAwake : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabToSpawn;

    void Awake()
    {
        var go = Instantiate(m_PrefabToSpawn);
        var agent = go.GetComponentInChildren<NavMeshAgent>();
        if(agent)
        {
            agent.Warp(transform.position);
        }
        else
        {
            go.transform.position = transform.position;
        }
    }

}
