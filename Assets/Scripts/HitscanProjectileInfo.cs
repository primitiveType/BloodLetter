using UnityEngine;

public class HitscanProjectileInfo : ProjectileInfoBase, IDamageSource
{
    [SerializeField] private float m_Damage;

    // public GameObject OnHitPrefab;
    public GameObject OnHitWallPrefab;

    protected ActorRoot ownerRoot;
    public bool Piercing;
    public float Damage => m_Damage;


    public Damage GetDamage()
    {
        return new Damage(Damage, Type);
    }

    public virtual void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, ActorRoot actorRoot)
    {
        ownerRoot = actorRoot;
        IDamagedByHitscanProjectile damaged = HitscanUtils.GetHitObject(ownerPosition, ownerDirection, actorRoot, Range,
            OnHitWallPrefab,
            out RaycastHit hit);
        damaged?.OnShot( hit.point, this, ownerDirection);
    }


    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        ownerRoot = actorRoot;
        TriggerShoot(owner.position, direction, actorRoot);
    }
}