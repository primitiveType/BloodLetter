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