using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantaneousAoeDamage : MonoBehaviour, IDamageSource
{
    [SerializeField] private float Radius;
    [SerializeField] private float DamageAmount;

    [SerializeField] private float Force;
    // Start is called before the first frame update
    void Start()
    {
        var hitObjects = Physics.OverlapSphere(transform.position, Radius, ~LayerMask.NameToLayer("Default"));
        foreach (Collider collider in hitObjects)
        {
            Debug.Log($"hit {collider.name}");
            var damaged = collider.GetComponent<ActorEvents>();
            if (collider.attachedRigidbody)
            {
                var direction = collider.transform.position - transform.position;
                collider.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
            }

            if (damaged)
            {
                damaged.OnShot(this);
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    public float GetDamage(ActorHealth actorToDamage)
    {
        return DamageAmount;
    }
}
