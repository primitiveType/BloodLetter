using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGravityOnDeath : MonoBehaviour
{
    [SerializeField] private ActorEvents Events;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
