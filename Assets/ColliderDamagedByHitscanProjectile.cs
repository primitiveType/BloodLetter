using System;
using UnityEngine;

public class ColliderDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    private ActorRoot Root;

    public Collider Collider { get; private set; }

    private void Awake()
    {
        Root = GetComponentInParent<ActorRoot>();
        Collider = GetComponent<Collider>();
    }

    public bool IsHit(Vector2 textureCoord)
    {
        return true;
    }

    public void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo, Vector3 hitNormal)
    {
        Root.ActorEvents.OnShot(projectileInfo, Collider.ClosestPoint(worldPos), hitNormal);
    }

    public void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        Root.ActorEvents.OnShot(projectileInfo, Collider.ClosestPoint(worldPos), hitNormal);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    private void Start()
    {
        if (Root == null) Root = GetComponent<ActorRoot>();
    }
}