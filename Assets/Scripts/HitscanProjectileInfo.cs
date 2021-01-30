using System.Linq;
using UnityEngine;

public class HitscanProjectileInfo : ProjectileInfoBase, IDamageSource
{
    [SerializeField] private float m_Damage;

    // public GameObject OnHitPrefab;
    public GameObject OnHitWallPrefab;

    protected ActorRoot ownerRoot;
    public bool Piercing;
    public float Damage => m_Damage;


    protected static LayerMask EnvironmentLayers =>
        LayerMask.GetMask("Default", "Interactable");

    public Damage GetDamage()
    {
        return new Damage(Damage, Type);
    }

    public void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, ActorRoot actorRoot)
    {
        ownerRoot = actorRoot;
        var damaged = GetHitObject(ownerPosition, ownerDirection, actorRoot, out var hit);
        if (damaged != null && !ownerRoot.HitscanColliders.Contains(damaged))
        {
            damaged.OnShot(hit.textureCoord, hit.point, this, ownerDirection);
        }
    }

    protected virtual GameObject CreateHitEffect(GameObject prefab, Transform parent, RaycastHit hit)
    {
        var hitEffect = Instantiate(prefab, parent, true);
        return hitEffect;
    }

    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        ownerRoot = actorRoot;
        TriggerShoot(owner.position, direction, actorRoot);
    }


    protected IDamagedByHitscanProjectile GetHitObject(Vector3 ownerPosition, Vector3 ownerDirection,
        ActorRoot actorRoot, out RaycastHit hit)
    {
        var ray = new Ray(ownerPosition, ownerDirection * Range);
        Debug.DrawRay(ownerPosition, ownerDirection * Range, Color.blue, 10);
        var layerToCheckForDamage = GetLayerToCheckForDamage(actorRoot.EntityType);

        var raycastMask = GetRaycastMask(actorRoot.EntityType);

        var isDone = false;
        hit = new RaycastHit();

        var maxChecks = 10;
        var checks = 0;
        while (!isDone)
            if (Physics.Raycast(ray, out hit, Range, raycastMask, QueryTriggerInteraction.Ignore))
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
                else if (OnHitWallPrefab) //we did hit the environment, spawn a hit visual and quit.
                {
                    var hitRotation = Quaternion.LookRotation(-hit.normal);

                    var hitEffect = CreateHitEffect(OnHitWallPrefab, hit.collider.transform, hit);
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


    protected static int GetRaycastMask(EntityType ownerType)
    {
        //always ignore this one, since its the enemy collider which we don't use for raycasts
        var raycastMask = ~LayerMask.GetMask("Enemy");

        if (ownerType == EntityType.Player)
            //HACK to make player unable to shoot their feet lol
            raycastMask &= ~LayerMask.GetMask("Player");

        raycastMask &= ~LayerMask.GetMask("Ignore Raycast");
        return raycastMask;
    }

    protected static LayerMask GetLayerToCheckForDamage(EntityType ownerType)
    {
        var actorLayer = ownerType == EntityType.Player
            ? "EnemyRaycast"
            : "Player";

        return LayerMask.GetMask("Destructible", actorLayer);
    }
}