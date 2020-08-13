﻿using UnityEngine;

public class ScatterProjectileInfo : ProjectileInfoBase
{
    [SerializeField] private HitscanProjectileInfo projectileInfo;
    [SerializeField] private float degreesScatter;
    [SerializeField] private int numProjectiles;

    public void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, ActorRoot actorRoot)
    {
        //assume up vector is y axis
        for (int i = 0; i < numProjectiles; i++)
        {
            var x = Random.Range(-degreesScatter, degreesScatter);
            var y = Random.Range(-degreesScatter, degreesScatter);
            var z = Random.Range(-degreesScatter, degreesScatter);
            var position = playerDirection +
                           new Vector3(x, y, z);
            projectileInfo.TriggerShoot(playerPosition, position, actorRoot);
        }
    }

    public override void TriggerShoot(Transform owner, Vector3 direction,  ActorRoot actorRoot)
    {
        TriggerShoot(owner.position, direction, actorRoot);
    }

}