using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedByHitscanProjectile : MonoBehaviour
{
    [SerializeField] private AnimationMaterialHelper MaterialHelper;
    [SerializeField] private ActorEvents Events;

    public bool OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo)
    {
        if (!MaterialHelper.QueryAlpha(textureCoord))
        {
            return false;
        }

        Events.OnShot(projectileInfo);
        return true;
    }
}