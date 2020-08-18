using UnityEngine;

public abstract class PlayerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //check for player?
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Trigger(other);
        }
    }

    protected abstract void Trigger(Collider other);
}