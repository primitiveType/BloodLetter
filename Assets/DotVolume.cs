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

    private bool TryAddTrigger(Collider other)
    {
        var actor = other.GetComponent<IActorEvents>();
        if (actor != null && !TargetsByTime.ContainsKey(other))
        {
            TargetsByTime.Add(other, 0);
            return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision other)
    {
        TryAddTrigger(other.collider);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsDirectlyAbove(other))
        {//we are standing on a platform
            if (TargetsByTime.ContainsKey(other))
            {
                //player isn't just jumping, they are in a safe zone. remove them from consideration
                TargetsByTime.Remove(other);
            }
            return;
        }

        if (IsGroundedOn(other))
        {//we are standing on the hazard
            TryAddTrigger(other);
        }
        
        if (!(TargetsByTime.ContainsKey(other)))
        {
            return;
        }

        //Player is standing on hazard or jumping around on it
        TargetsByTime[other] += Time.deltaTime;
        if (TargetsByTime[other] > SecondsPerTick)
        {
            TargetsByTime[other] -= SecondsPerTick;
            var actor = other.GetComponent<IActorEvents>();
            actor.OnShot(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TargetsByTime.ContainsKey(other))
        {
            TargetsByTime.Remove(other);
        }
    }

    public Damage GetDamage(ActorHealth actorToDamage)
    {
        return new Damage(Damage, DamageType.Hazard);
    }

    private bool IsDirectlyAbove(Collider other)
    {
        Physics.Raycast(other.transform.position, Vector3.down, out var hit, LayerMask.GetMask("Default", "Hazard"));
        if (hit.collider.gameObject == this.gameObject)
        {
            return true;
        }

        return false;
    }

    private bool IsGroundedOn(Collider other)
    {
        var bounds = other.bounds;
        Vector3 origin = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);;
        Physics.Raycast(origin, Vector3.down, out var hit, LayerMask.GetMask("Default", "Hazard"));
        if (hit.collider.gameObject != this.gameObject)
        {
            return false;
        }

        if (hit.distance > .1f) //that's a heck of an assumption
            return false;

        return true;
    }
}