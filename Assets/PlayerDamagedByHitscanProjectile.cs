using UnityEngine;

public class PlayerDamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private PlayerEvents Events;

    public bool IsHit(Vector2 textureCoord)
    {
        return true;
    }

    public void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo, Vector3 hitNormal)
    {
        Events.OnShot(projectileInfo, worldPos, hitNormal);
    }

    public void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        Events.OnShot(projectileInfo, worldPos, hitNormal);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}