using UnityEngine;

public interface IDamagedByHitscanProjectile
{
    Transform transform { get; }
    bool IsHit(Vector2 textureCoord);
    void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo);
    void OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos);
    void SetEnabled(bool enabled);
}