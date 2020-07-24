using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private PlayerEvents Events;

    public bool OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
        return true;
    }

    public bool OnShot(HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
        return true;
    }
}

public interface IDamagedByHitscanProjectile 
{
    bool OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo);
    bool OnShot(HitscanProjectileInfo projectileInfo);
    Transform transform { get; }
}
