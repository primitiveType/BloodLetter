using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnShot : MonoBehaviour
{
    private IActorEvents Events;
    [SerializeField] private Animator Animator;
    private static readonly int Trigger = Animator.StringToHash("Trigger");

    private void Start()
    {
        Events = GetComponent<IActorEvents>();
        Events.OnShotEvent += EventsOnOnShotEvent;
    }

    private void EventsOnOnShotEvent(object sender, OnShotEventArgs args)
    {
        Animator.SetTrigger(Trigger);
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= EventsOnOnShotEvent;
    }
}