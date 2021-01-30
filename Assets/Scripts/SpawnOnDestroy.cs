using UnityEngine;
using UnityEngine.AI;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabToSpawn;
    [SerializeField] private bool m_InheritScale;

    void OnDestroy()
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

        if (m_InheritScale)
        {
            go.transform.localScale = transform.localScale;
        }
    }

}