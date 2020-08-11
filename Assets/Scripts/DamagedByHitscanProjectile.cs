using System;
using System.Collections;
using System.Collections.Generic;
using C5;
using UnityEngine;

public class DamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    private AnimationMaterialHelper MaterialHelper => ActorRoot.AnimationMaterialHelper;
    private IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot { get; set; }

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
    }

    public virtual bool OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo)
    {
        if (!MaterialHelper.QueryAlpha(textureCoord))
        {
            return false;
        }

        Events.OnShot(projectileInfo);
        return true;
    }

    public bool OnShot(HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
        return true;
    }
}