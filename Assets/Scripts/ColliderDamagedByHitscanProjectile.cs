using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        return enabled;
    }

    public void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo, Vector3 hitNormal)
    {
        Vector3 randomVector = new Vector3((Random.value - .5f) * 1000, (Random.value - .5f) * 1000, (Random.value - .5f) * 1000);
        Root.ActorEvents.OnShot(projectileInfo, Collider.ClosestPoint(randomVector), hitNormal);
    }

    public void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        Vector3 randomVector = new Vector3((Random.value - .5f) * 1000, (Random.value - .5f) * 1000, (Random.value - .5f) * 1000);
        Root.ActorEvents.OnShot(projectileInfo, Collider.ClosestPoint(randomVector), hitNormal);
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