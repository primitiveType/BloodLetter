using System;
using System.Collections.Generic;
using UnityEngine;

public class TeleportVolume : MonoBehaviour
{
    [SerializeField] private GameObject TargetPosition;
    [SerializeField] private Transform Pivot;


    private List<Collider> GameObjectsToIgnore = new List<Collider>();

    [SerializeField] private LayerMask LayerMask;

    private void Awake()
    {
        if (LayerMask == 0)
        {
            LayerMask = LayerMask.GetMask("Player", "PlayerProjectile", "Enemy", "DeadEnemy");
        }
    }

    //called if this teleporter is used as a target for another teleporter
    public void Receive(ActorRoot actor, Collider collider, Rigidbody rb, Vector3 relativeVelocity)
    {
        if (!GameObjectsToIgnore.Contains(collider))
        {
            GameObjectsToIgnore.Add(collider);
        }

        if (rb != null)
        {
            Vector3 rbVelocity = transform.TransformDirection(relativeVelocity);
            rb.velocity = Vector3.Reflect(rbVelocity, transform.forward);
        }

        if (actor != null)
        {
            Teleport(actor, Pivot.position, Pivot.rotation);
        }
        else
        {
            Teleport(collider.transform, Pivot.position, Pivot.rotation);
        }
    }

    private void Teleport(ActorRoot actor, Vector3 transformPosition, Quaternion transformRotation)
    {
        if (actor.Navigation != null)
        {
            actor.Navigation.Warp(transformPosition);
        }
        else
        {
            actor.transform.SetPositionAndRotation(transformPosition, transformRotation);
        }
    }

    private void Teleport(Transform t, Vector3 transformPosition, Quaternion transformRotation)
    {
        t.SetPositionAndRotation(transformPosition, transformRotation);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObjectsToIgnore.Remove(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!LayerMask.ContainsLayer(other.gameObject.layer))
        {
            return;
        }

        if (GameObjectsToIgnore.Contains(other))
        {
            return;
        }

        var actor = other.GetComponentInParent<ActorRoot>();

        var teleporter = TargetPosition.GetComponent<TeleportVolume>();
        var rb = other.GetComponentInParent<Rigidbody>();
        var localVelocity = rb != null ? transform.InverseTransformDirection(rb.velocity) : Vector3.zero;
        if (teleporter)
        {
            teleporter.Receive(actor, other, rb, localVelocity);
        }
        else
        {
            Teleport(actor, TargetPosition.transform.position, TargetPosition.transform.rotation);
        }
    }
}