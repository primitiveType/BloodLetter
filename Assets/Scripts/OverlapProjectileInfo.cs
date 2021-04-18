using UnityEngine;

public class OverlapProjectileInfo : HitscanProjectileInfo
{
    private readonly Collider[] hits = new Collider[4];
    [SerializeField] private float m_Radius = 1;
    [SerializeField] private Vector3 m_Offset;

    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot, GameObject target)
    {
        Transform ownerTransform = owner.transform;
        var position = ownerTransform.TransformPoint(m_Offset);
        var victims = HitscanUtils.CheckOverlap(position, actorRoot, hits, m_Radius);

        foreach (var damaged in victims)
            damaged.OnShot(this, damaged.transform.position, direction);
    }
}