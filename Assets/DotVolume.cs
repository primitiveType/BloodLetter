using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DotVolume : MonoBehaviour, IDamageSource
{
    private Dictionary<Collider, float> TargetsByTime = new Dictionary<Collider, float>();

    [SerializeField] private int Damage = 1;

    [SerializeField] private float TicksPerSecond = 1;

    private float SecondsPerTick => 1f / TicksPerSecond;
    private void OnTriggerEnter(Collider other)
    {
   
    }


    private bool TryAddTrigger(Collider other)
    {
        var actor = other.GetComponent<ActorEvents>();
        if (actor && !TargetsByTime.ContainsKey(other))
        {
            TargetsByTime.Add(other, 0);
            return true;
        }

        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!TargetsByTime.ContainsKey(other) && !TryAddTrigger(other))
        {
            return;
        }
        
        TargetsByTime[other] += Time.deltaTime;
        if (TargetsByTime[other] > SecondsPerTick )
        {
            TargetsByTime[other] -= SecondsPerTick;
            var actor = other.GetComponent<ActorEvents>();
            actor.OnShot(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //TargetsByTime.Remove(other);
    }

    public float GetDamage(ActorHealth actorToDamage)
    {
        return Damage;
    }
}
