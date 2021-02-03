using UnityEngine;

public interface IDamagedByHitscanProjectile
{
    ActorRoot ActorRoot { get; }
    Transform transform { get; }
    bool IsHit(Vector2 textureCoord);
    void OnShot(Vector3 worldPos, IDamageSource projectileInfo, Vector3 hitNormal);
    void OnShot(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal);
    void SetEnabled(bool enabled);
}