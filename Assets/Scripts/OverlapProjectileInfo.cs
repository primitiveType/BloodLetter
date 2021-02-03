using UnityEngine;

public class OverlapProjectileInfo : HitscanProjectileInfo
{
    private readonly Collider[] hits = new Collider[4];

    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        var victims = HitscanUtils.CheckOverlap(owner.transform.position, actorRoot, hits, 1);

        foreach (var damaged in victims)
            damaged.OnShot(this, damaged.transform.position, direction);
    }
}