using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpawnOnInteract : BaseInteractable
{
    [SerializeField] private GameObject m_PrefabToSpawn;
    [SerializeField] private float m_Delay;

    private bool triggered;

    protected override bool DoInteraction()
    {
        if (!isActiveAndEnabled)
        {
            return false;
        }
        if (triggered)
        {
            return false;
        }

        triggered = true;
        StartCoroutine(SpawnAfterDelay());
        return true;
    }

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(m_Delay);
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

    private void Update()
    {
    }
}