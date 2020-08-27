using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject Interactable;

    [SerializeField] private ActorHealth Health;

    // Start is called before the first frame update
    void Start()
    {
        Health.Events.OnDeathEvent += OnDeath;
    }

    private void OnDeath(object sender, OnDeathEventArgs args)
    {
        foreach (var interactable in Interactable.GetComponents<IInteractable>())
        {
            interactable.Interact();
        }
    }

    private void OnDestroy()
    {
        Health.Events.OnDeathEvent -= OnDeath;
    }

}