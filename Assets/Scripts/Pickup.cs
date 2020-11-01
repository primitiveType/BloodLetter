using UnityEngine;

public abstract class Pickup<T> : MonoBehaviour where T : Component
{
    private AudioSource audioSource;
    protected T currentActor;
    private Collider currentCollider;

    public bool toast = true;

    protected abstract string toastMessage { get; }

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
            if (toastMessage != null && toast) ToastHandler.Instance.PopToast(toastMessage);

            PlaySound();
            PickupItem();
            Destroy(gameObject);
        }
    }

    private void PlaySound()
    {
        if (audioSource)
            audioSource.PlayOneShot(audioSource.clip);
    }

    protected abstract bool CanBePickedUp();

    protected abstract void PickupItem();
}