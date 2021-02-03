using System;
using UnityEngine;

public class PlayerDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private PlayerEvents Events;

    public ActorRoot ActorRoot { get; private set; }

    private void Awake()
    {
        ActorRoot = GetComponentInParent<PlayerRoot>();
    }

    public bool IsHit(Vector2 textureCoord)
    {
        return true;
    }

    public void OnShot(Vector3 worldPos, IDamageSource projectileInfo, Vector3 hitNormal)
    {
        Events.OnShot(projectileInfo, worldPos, hitNormal);
    }

    public void OnShot(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        Events.OnShot(projectileInfo, worldPos, hitNormal);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}