using UnityEngine;

public class ColliderDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private IActorEvents Events;

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