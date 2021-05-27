using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HitscanUtils
{
    public static LayerMask EnvironmentLayers =>
        LayerMask.GetMask("Default", "Interactable");

    public static IDamagedByHitscanProjectile GetHitObject(Vector3 ownerPosition, Vector3 ownerDirection,
        ActorRoot actorRoot, float range, GameObject onHitWallPrefab, out RaycastHit hit)
    {
        var ray = new Ray(ownerPosition, ownerDirection * range);
        Debug.DrawRay(ownerPosition, ownerDirection * range, Color.blue, 10);
        var layerToCheckForDamage = GetLayerToCheckForDamage(actorRoot.EntityType);

        var raycastMask = GetRaycastMask(actorRoot.EntityType);

        var isDone = false;
        hit = new RaycastHit();

        var maxChecks = 10;
        var checks = 0;
        while (!isDone)
            if (Physics.Raycast(ray, out hit, range, raycastMask, QueryTriggerInteraction.Ignore))
            {
                var potentialHit = false;
                checks++;
                var hitLayer = hit.collider.gameObject.layer;
                if (((1 << hitLayer) & layerToCheckForDamage) != 0) potentialHit = true;

                if (potentialHit) //we hit something that can be damaged, but could be self
                {
                    var hitCoord = hit.textureCoord;
                    var damaged = hit.collider.GetComponent<IDamagedByHitscanProjectile>();
                    if (damaged != null && !actorRoot.HitscanColliders.Contains(damaged) && damaged.IsHit(hitCoord))
                    {
//                        Debug.Log($"{actorRoot} hit {damaged}");
                        return damaged;
                    }
                }

                //if we got here, the raycast was not considered a hit on something that can be damaged.

                if (((1 << hitLayer) & EnvironmentLayers) == 0) //we didn't hit the environment, so keep moving forward
                {
                    ray.origin = hit.point + hit.normal * -.01f;
                }
                else if (onHitWallPrefab) //we did hit the environment, spawn a hit visual and quit.
                {
                    var hitRotation = Quaternion.LookRotation(-hit.normal);

                    var hitEffect = CreateHitEffect(onHitWallPrefab, hit.collider.transform, hit);
                    hitEffect.transform.rotation = hitRotation;

                    var adjustmentDistance = .001f;

                    hitEffect.transform.position = hit.point + hit.normal * adjustmentDistance;

                    // hitEffect.transform.localRotation = hitRotation;
                    isDone = true;
                }
                else //not sure this ever happens?
                {
                    isDone = true;
                }

                if (checks > maxChecks) //HACK
                    isDone = true;
            }
            else //we hit nothing. stop raycasting.
            {
                isDone = true;
            }

        return null;
    }

    public static List<IDamagedByHitscanProjectile> CheckOverlap(Vector3 position,
        ActorRoot actorRoot, Collider[] hits, float radius)
    {
        var layerToCheckForDamage = HitscanUtils.GetLayerToCheckForDamage(actorRoot.EntityType);

        var raycastMask = LayerMask.GetMask("Enemy", "DeadEnemy");
        var size = Physics.OverlapSphereNonAlloc(position, radius, hits, raycastMask);
        var isDone = false;
        bool hitSomething = false;
        //todo: non alloc version
        List<IDamagedByHitscanProjectile> victims = new List<IDamagedByHitscanProjectile>();
        for (var i = 0; i < size; i++)
        {
            if (isDone) break;

            var hit = hits[i];
            var hitLayer = hit.gameObject.layer;

            var damaged = hit.GetComponent<IDamagedByHitscanProjectile>();
            if (damaged != null && damaged.IsHit(Vector2.negativeInfinity))
            {
                // damaged.OnShot(projectileInfo, hit.ClosestPoint(position), direction);
                //isDone = true;
                victims.Add(damaged);
            }

            // else if (onHitWallPrefab)
            // {
            //     var hitRotation = Quaternion.LookRotation(-direction);
            //     var transform1 = hit.transform;
            //     var hitEffect = Object.Instantiate(onHitWallPrefab, transform1.position, hitRotation, transform1);
            //
            //     var adjustmentDistance = .001f;
            //
            //     hitEffect.transform.position = hit.ClosestPoint(ownerPosition) + direction * adjustmentDistance;
            // }
        }

        return victims;
    }

    public static GameObject CreateHitEffect(GameObject prefab, Transform parent, RaycastHit hit)
    {
        var hitEffect = Object.Instantiate(prefab, parent, true);
        return hitEffect;
    }

    public static int GetRaycastMask(EntityType ownerType)
    {
        //always ignore this one, since its the enemy collider which we don't use for raycasts
        var raycastMask = ~LayerMask.GetMask("Enemy");

        if (ownerType == EntityType.Player)
            //HACK to make player unable to shoot their feet lol
            raycastMask &= ~LayerMask.GetMask("Player");

        if (ownerType == EntityType.Enemy)
        {
            raycastMask &= ~LayerMask.GetMask("EnemyRaycast");
        }

        raycastMask &= ~LayerMask.GetMask("Ignore Raycast");
        return raycastMask;
    }

    public static LayerMask GetLayerToCheckForDamage(EntityType ownerType)
    {
        var actorLayer = ownerType == EntityType.Player
            ? "EnemyRaycast"
            : "Player";

        return LayerMask.GetMask("Destructible", actorLayer);
    }
}