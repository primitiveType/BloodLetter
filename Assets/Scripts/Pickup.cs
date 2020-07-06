using System;
using UnityEngine;

public abstract class Pickup<T> : MonoBehaviour where T : Component
{
    private Collider currentCollider;
    protected T currentActor;

    protected abstract string toastMessage { get; }

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
            if (toastMessage != null)
            {
                ToastHandler.Instance.PopToast(toastMessage);
            }

            PickupItem();
            Destroy(gameObject);
        }
    }

    protected abstract bool CanBePickedUp();

    protected abstract void PickupItem();
}