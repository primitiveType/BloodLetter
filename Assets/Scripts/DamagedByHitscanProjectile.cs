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

    public bool IsHit(Vector2 textureCoord)
    {
        if (!enabled)
        {
            return false;
        }

        return MaterialHelper.QueryAlpha(textureCoord);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public virtual void OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
    }

    public void OnShot(HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
    }
}