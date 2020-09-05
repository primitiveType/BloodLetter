using System;
using CodingEssentials;
using UnityEngine;

public abstract class Pickup<T> : MonoBehaviour where T : Component
{
    private Collider currentCollider;
    protected T currentActor;

    public bool toast = true;

    protected abstract string toastMessage { get; }

    private AudioSource audioSource;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Pickup");
        audioSource = GetComponent<AudioSource>();
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
            if (toastMessage != null && toast)
            {
                ToastHandler.Instance.PopToast(toastMessage);
            }

            PlaySound();
            PickupItem();
            Destroy(gameObject);
        }
    }

    private void PlaySound()
    {
        if (audioSource)
            audioSource.Play();
    }

    protected abstract bool CanBePickedUp();

    protected abstract void PickupItem();
}