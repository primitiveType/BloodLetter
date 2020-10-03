using UnityEngine;

public class OverlapProjectileInfo : HitscanProjectileInfo
{
    private readonly Collider[] hits = new Collider[4];

    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        var ownerPosition = owner.position;
        Debug.DrawLine(ownerPosition, direction);
        var layerToCheckForDamage = GetLayerToCheckForDamage(actorRoot.EntityType);

        var raycastMask = GetRaycastMask(actorRoot.EntityType);
        var size = Physics.OverlapSphereNonAlloc(ownerPosition + direction, 1, hits, raycastMask);
        var isDone = false;
        for (var i = 0; i < size; i++)
        {
            if (isDone) break;

            var hit = hits[i];
            var hitLayer = hit.gameObject.layer;

            var damaged = hit.GetComponent<IDamagedByHitscanProjectile>();
            if (damaged != null)
            {
                damaged.OnShot(this, hit.ClosestPoint(ownerPosition), direction);
                //isDone = true;
            }
            else if (OnHitWallPrefab)
            {
                var hitRotation = Quaternion.LookRotation(-direction);
                var transform1 = hit.transform;
                var hitEffect = Instantiate(OnHitWallPrefab, transform1.position, hitRotation, transform1);

                var adjustmentDistance = .001f;

                hitEffect.transform.position = hit.ClosestPoint(ownerPosition) + direction * adjustmentDistance;
            }
        }
    }
}