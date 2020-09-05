using System;
using System.Collections;
using System.Collections.Generic;
using C5;
using UnityEngine;

public class DamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private ActorRoot _actorRoot;
    private AnimationMaterialHelper MaterialHelper => ActorRoot.AnimationMaterialHelper;
    private IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot
    {
        get => _actorRoot;
        set => _actorRoot = value;
    }

    private void Start()
    {
        if (ActorRoot == null)
        {
            ActorRoot = GetComponentInParent<ActorRoot>();
        }
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