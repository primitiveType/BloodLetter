using System;
using UnityEngine;

public abstract class Pickup<T> : MonoBehaviour where T : Component
{
    private Collider currentCollider;
    protected T currentActor;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Pickup");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != currentCollider)
        {
            currentCollider = other;
            currentActor = other.GetComponent<T>();
        }

        if (currentActor != null && CanBePickedUp())
        {
            PickupItem();
            Destroy(gameObject);
        }
    }

    protected abstract bool CanBePickedUp();

    protected abstract void PickupItem();
}