using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private AnimationMaterialHelper MaterialHelper;
    [SerializeField] private ActorEvents Events;

    public virtual bool OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo)
    {
        if (!MaterialHelper.QueryAlpha(textureCoord))
        {
            return false;
        }

        Events.OnShot(projectileInfo);
        return true;
    }
}