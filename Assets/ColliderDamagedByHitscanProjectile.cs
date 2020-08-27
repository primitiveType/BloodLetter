using System;
using UnityEngine;

public class ColliderDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private IActorEvents Events;

    private void Start()
    {
        if (Events == null)
        {
            Events = GetComponent<IActorEvents>();
        }
    }

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

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}