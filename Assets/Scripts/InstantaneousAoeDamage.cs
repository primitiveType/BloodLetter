﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantaneousAoeDamage : MonoBehaviour, IDamageSource
{
    [SerializeField] private float Radius;
    [SerializeField] private float DamageAmount;

    [SerializeField] private float Force;

    // Start is called before the first frame update
    // void Start()
    // {
    //     var hitObjects = Physics.OverlapSphere(transform.position, Radius, ~LayerMask.NameToLayer("Default"));
    //     foreach (Collider collider in hitObjects)
    //     {
    //         Debug.Log($"hit {collider.name}");
    //         var damaged = collider.GetComponent<IActorEvents>();
    //         if (collider.attachedRigidbody)
    //         {
    //             var direction = collider.transform.position - transform.position;
    //             collider.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
    //         }
    //
    //         damaged?.OnShot(this);
    //     }
    // }
    
    private List<Collider> hitObjects = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (hitObjects.Contains(other))
        {
            return;
        }
        
        hitObjects.Add(other);
        
        Debug.Log($"hit {other.name}");
        var damaged = other.GetComponent<IActorEvents>();
        if (other.attachedRigidbody)
        {
            var direction = other.transform.position - transform.position;
            other.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
        }

        damaged?.OnShot(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

  
    public Damage GetDamage(ActorHealth hitActor)
    {
        return new Damage(DamageAmount, DamageType.Attack);
    }
}