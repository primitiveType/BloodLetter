using System;
using UnityEngine;

public class InteractOnDeath : MonoBehaviour
{
    [SerializeField] private ActorHealth Health;
    [SerializeField] private GameObject Interactable;

    // Start is called before the first frame update
    private void Start()
    {
        Health.Events.OnDeathEvent += OnDeath;
    }

    private void OnDeath(object sender, OnDeathEventArgs args)
    {
        foreach (var interactable in Interactable.GetComponents<IInteractable>()) interactable.Interact();
    }

    private void OnDestroy()
    {
        Health.Events.OnDeathEvent -= OnDeath;
    }
}