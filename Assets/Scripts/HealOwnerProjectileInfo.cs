using UnityEngine;

public class HealOwnerProjectileInfo : ProjectileInfoBase
{
    [SerializeField] private float HealAmount;
    private float lastShootTimestamp = 0;
    private float timeBetweenShots = .1f;

    public override bool TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot, GameObject target)
    {
        if (lastShootTimestamp > Time.time) //HACK
            lastShootTimestamp = -1;
        
        
        bool canShoot = Time.time - lastShootTimestamp > timeBetweenShots;

        if (canShoot)
        {
            lastShootTimestamp = Time.time;
            actorRoot.Health.Heal(HealAmount);
            return true;
        }

        return false;
    }
}