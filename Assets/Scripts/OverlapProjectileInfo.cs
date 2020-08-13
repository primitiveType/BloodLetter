using UnityEngine;

public class OverlapProjectileInfo : HitscanProjectileInfo
{
    private Collider[] hits = new Collider[4];
    public override void TriggerShoot(Transform owner, Vector3 direction,  ActorRoot actorRoot)
    {
        var ownerPosition = owner.position;
        Debug.DrawLine(ownerPosition, direction);
        var layerToCheckForDamage = GetLayerToCheckForDamage(actorRoot.EntityType);

        var raycastMask = GetRaycastMask(actorRoot.EntityType);
        var size = Physics.OverlapSphereNonAlloc(ownerPosition+direction, 1, hits, raycastMask);
        bool isDone = false;
        for (int i = 0; i < size; i++)
        {
            if (isDone)
            {
                break;
            }

            var hit = hits[i];
            int hitLayer = hit.gameObject.layer;
            // if (((hitLayer & layerToCheckForDamage) == 0))
            // {
            //     //isDone = true;
            // }

            // Debug.Log($"hit {hit.textureCoord} ");

            IDamagedByHitscanProjectile damaged = hit.GetComponent<IDamagedByHitscanProjectile>();
            if (damaged != null )
            {
                damaged.OnShot(this);
                isDone = true;
                var hitEffect = GameObject.Instantiate(OnHitPrefab, damaged.transform, true);

                float adjustmentDistance = .1f;

                hitEffect.transform.position = hit.ClosestPoint(ownerPosition) + (direction * adjustmentDistance);
            }
            else if(OnHitWallPrefab)
            {
                var hitRotation = Quaternion.LookRotation(-direction);
                var transform1 = hit.transform;
                var hitEffect = GameObject.Instantiate(OnHitWallPrefab, transform1.position, hitRotation, transform1 );

                float adjustmentDistance = .001f;

                hitEffect.transform.position = hit.ClosestPoint(ownerPosition) + (direction * adjustmentDistance);
                
            }
        }
    }
}