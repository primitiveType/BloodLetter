using System;
using UnityEngine;

[Serializable]
public class AttackData //first pass
{
    private Collider[] hitResults = new Collider[10];
    public float Radius;
    public float Range;

    public void DoAttack(Transform target)
    {
        int size = Physics.OverlapSphereNonAlloc(target.position, Radius, hitResults, LayerMask.GetMask("Player"),
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < size; i++)
        {
            Debug.Log($"Hit {hitResults[i]}");
        }
    }
}