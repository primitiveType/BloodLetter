using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private PlayerEvents Events;

    public bool IsHit(Vector2 textureCoord)
    {
        return true;
    }

    public void OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
    }

    public void OnShot(HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo);
    }
}

public interface IDamagedByHitscanProjectile
{
    bool IsHit(Vector2 textureCoord);
    void OnShot(Vector2 textureCoord, HitscanProjectileInfo projectileInfo);
    void OnShot(HitscanProjectileInfo projectileInfo);
    Transform transform { get; }
}
