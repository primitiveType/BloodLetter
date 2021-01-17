using UnityEngine;

public interface IDamagedByHitscanProjectile
{
    Transform transform { get; }
    bool IsHit(Vector2 textureCoord);
    void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo, Vector3 hitNormal);
    void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos, Vector3 hitNormal);
    void SetEnabled(bool enabled);
}