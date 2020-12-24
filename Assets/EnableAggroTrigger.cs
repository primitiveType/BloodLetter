using System.Collections.Generic;
using CodingEssentials.Collections;
using UnityEngine;

public class EnableAggroTrigger : PlayerTrigger
{
    [SerializeField] private List<ActorRoot> actorsToEnable;


    protected override void Trigger(Collider other)
    {
        foreach (var actorRoot in actorsToEnable)
        {
            actorRoot.HitscanColliders.ForEach(hsc=>hsc.SetEnabled(true));
            actorRoot.AggroHandler.enabled = true;
            actorRoot.AggroHandler.IsAggro = true;
            actorRoot.GetComponent<OnShotApplyForce>().enabled = true;
        }
    }
}