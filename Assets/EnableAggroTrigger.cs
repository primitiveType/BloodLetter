using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAggroTrigger : PlayerTrigger
{
    [SerializeField] private List<ActorRoot> actorsToEnable;


    protected override void Trigger(Collider other)
    {
        foreach (var actorRoot in actorsToEnable)
        {
            actorRoot.HitscanCollider.SetEnabled(true);
            actorRoot.AggroHandler.enabled = true;
        }
    }
}