using System;
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

    public void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo)
    {
        Events.OnShot(projectileInfo, worldPos);
    }

    public void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos)
    {
        Events.OnShot(projectileInfo, worldPos);
    }
    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}

public class OnShotParticleSpawner : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;
    [SerializeField] private GameObject OnHitPrefab;
    
    private void Start()
    {
        Events.OnShotEvent += OnShot;
    }

    private void OnShot(object sender, OnShotEventArgs args)
    {
        if (args.ProjectileInfo.GetDamage().Type.HasFlag(DamageType.Physical))
        {
            var hitEffect = CreateHitEffect(OnHitPrefab, transform);
            float adjustmentDistance = .1f;
            hitEffect.transform.position = args.WorldPos + (-transform.forward * adjustmentDistance);
        }
    }
    protected GameObject CreateHitEffect(GameObject prefab, Transform parent)
    {
        var hitEffect = GameObject.Instantiate(prefab, parent, true);
        return hitEffect;
    }
}

public interface IDamagedByHitscanProjectile
{
    bool IsHit(Vector2 textureCoord);
    void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo);
    void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos);
    Transform transform { get; }
    void SetEnabled(bool enabled);
}
