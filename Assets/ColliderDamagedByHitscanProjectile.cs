using UnityEngine;

public class ColliderDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private IActorEvents Events;

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