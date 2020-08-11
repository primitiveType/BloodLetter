using UnityEngine;

public class OverlapProjectileInfo : HitscanProjectileInfo
{
    private Collider[] hits = new Collider[4];
    public override void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, EntityType ownerType)
    {
        Debug.DrawLine(ownerPosition, ownerDirection);
        var layerToCheckForDamage = GetLayerToCheckForDamage(ownerType);

        var raycastMask = GetRaycastMask(ownerType);
        var size = Physics.OverlapSphereNonAlloc(ownerPosition+ownerDirection, 1, hits, raycastMask);
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

                hitEffect.transform.position = hit.ClosestPoint(ownerPosition) + (ownerDirection * adjustmentDistance);
            }
            else if(OnHitWallPrefab)
            {
                var hitRotation = Quaternion.LookRotation(-ownerDirection);
                var transform1 = hit.transform;
                var hitEffect = GameObject.Instantiate(OnHitWallPrefab, transform1.position, hitRotation, transform1 );

                float adjustmentDistance = .001f;

                hitEffect.transform.position = hit.ClosestPoint(ownerPosition) + (ownerDirection * adjustmentDistance);
                
            }
        }
    }
}