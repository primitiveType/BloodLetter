using UnityEngine;

public class TargetAllVisibleEnemiesProjectileInfo : ProjectileInfo
{
    [SerializeField] private ShootDirectionProvider DirectionProvider;
    public override bool TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot, GameObject target)
    {
        foreach (var newDirection in DirectionProvider.AllShootDirections)
        {
            base.TriggerShoot(owner, newDirection, actorRoot, target);
        }

        return true;
    }
}