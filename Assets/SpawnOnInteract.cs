using System.Collections;
using UnityEngine;

public class SpawnOnInteract : BaseInteractable
{
    [SerializeField] private GameObject m_PrefabToSpawn;
    [SerializeField] private float m_Delay;

    private bool triggered;

    protected override bool DoInteraction()
    {
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
        Instantiate(m_PrefabToSpawn, transform);
    }
}