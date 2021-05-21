using UnityEngine;

public class HealOwnerProjectileInfo : ProjectileInfoBase
{
    [SerializeField] private float HealAmount;
    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot, GameObject target)
    {
        actorRoot.Health.Heal(HealAmount);
    }
}