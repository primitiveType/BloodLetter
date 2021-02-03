using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColliderDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    public ActorRoot ActorRoot { get; private set; }

    public Collider Collider { get; private set; }

    private void Awake()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        Collider = GetComponent<Collider>();
    }

    public bool IsHit(Vector2 textureCoord)
    {
        return enabled;
    }

    public void OnShot(Vector3 worldPos, IDamageSource projectileInfo, Vector3 hitNormal)
    {
        Vector3 randomVector = new Vector3((Random.value - .5f) * 1000, (Random.value - .5f) * 1000,
            (Random.value - .5f) * 1000);
        ActorRoot.ActorEvents.OnShot(projectileInfo, Collider.ClosestPoint(randomVector), hitNormal);
    }

    public void OnShot(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        Vector3 randomVector = new Vector3((Random.value - .5f) * 1000, (Random.value - .5f) * 1000,
            (Random.value - .5f) * 1000);
        ActorRoot.ActorEvents.OnShot(projectileInfo, Collider.ClosestPoint(randomVector), hitNormal);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    private void Start()
    {
        if (ActorRoot == null) ActorRoot = GetComponent<ActorRoot>();
    }
}