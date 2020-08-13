using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class HitscanProjectileInfo : ProjectileInfoBase, IDamageSource
{
    public bool Piercing;
    public GameObject OnHitPrefab;
    public GameObject OnHitWallPrefab;
    [SerializeField] private float m_Damage;
    public float Damage => m_Damage;
    private static List<int> m_EnvironmentLayers;

    private static List<int> EnvironmentLayers =>
        m_EnvironmentLayers != null
            ? m_EnvironmentLayers
            : m_EnvironmentLayers = new List<int>()
            {
                LayerMask.NameToLayer("Default"),
                LayerMask.NameToLayer("Interactable")
            };


    public void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, ActorRoot actorRoot)
    {
        var damaged = GetHitObject(ownerPosition, ownerDirection, actorRoot, out RaycastHit hit);
        if (damaged != null && damaged != ownerRoot.HitscanCollider)
        {
            damaged.OnShot(hit.textureCoord, this);
            var hitEffect = GameObject.Instantiate(OnHitPrefab, damaged.transform, true);
            float adjustmentDistance = .1f;
            hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
        }
    }

    private ActorRoot ownerRoot;
    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        ownerRoot = actorRoot;
        TriggerShoot(owner.position, direction, actorRoot);
    }


    protected IDamagedByHitscanProjectile GetHitObject(Vector3 ownerPosition, Vector3 ownerDirection,
        ActorRoot actorRoot, out RaycastHit hit)
    {
        Ray ray = new Ray(ownerPosition, ownerDirection * Range);
        Debug.DrawRay(ownerPosition, ownerDirection * Range, Color.blue, 10);
        var layerToCheckForDamage = GetLayerToCheckForDamage(actorRoot.EntityType);

        var raycastMask = GetRaycastMask(actorRoot.EntityType);

        bool isDone = false;
        hit = new RaycastHit();

        int maxChecks = 10;
        int checks = 0;
        while (!isDone)
        {
            if (Physics.Raycast(ray, out hit, Range, raycastMask))
            {
                bool potentialHit = false;
                checks++;
                int hitLayer = hit.collider.gameObject.layer;
                if ((hitLayer | layerToCheckForDamage) == layerToCheckForDamage)
                {
                    potentialHit = true;
                }

                if (potentialHit)//we hit something that can be damaged, but could be self
                {
                    var hitCoord = hit.textureCoord;
                    IDamagedByHitscanProjectile damaged = hit.collider.GetComponent<IDamagedByHitscanProjectile>();
                    if (damaged != null && damaged != actorRoot.HitscanCollider && damaged.IsHit(hitCoord))
                    {
                        return damaged;
                    }
                }
                
                //if we got here, the raycast was not considered a hit on something that can be damaged.
                
                if (!EnvironmentLayers.Contains(hitLayer))//we didn't hit the environment, so keep moving forward
                {
                    ray.origin = hit.point + (hit.normal * -.01f);
                }
                else if (OnHitWallPrefab)//we did hit the environment, spawn a hit visual and quit.
                {
                    var hitRotation = Quaternion.LookRotation(-hit.normal);
                    var hitEffect = GameObject.Instantiate(OnHitWallPrefab, hit.collider.transform.position,
                        hitRotation, hit.transform);

                    float adjustmentDistance = .001f;

                    hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);

                    // hitEffect.transform.localRotation = hitRotation;
                    isDone = true;
                }
                else//not sure this ever happens?
                {
                    isDone = true;
                }

                if (checks > maxChecks)//HACK
                {
                    isDone = true;
                }
            }
            else//we hit nothing. stop raycasting.
            {
                isDone = true;
            }
        }

        return null;
    }


    protected static int GetRaycastMask(EntityType ownerType)
    {
        //always ignore this one, since its the enemy collider which we don't use for raycasts
        int raycastMask = ~LayerMask.GetMask("Enemy");

        if (ownerType == EntityType.Player)
        {
            //HACK to make player unable to shoot their feet lol
            raycastMask &= ~LayerMask.GetMask("Player");
        }

        return raycastMask;
    }

    protected static int GetLayerToCheckForDamage(EntityType ownerType)
    {
        int layerToCheckForDamage = ownerType == EntityType.Player
            ? LayerMask.NameToLayer("EnemyRaycast")
            : LayerMask.NameToLayer("Player");


        layerToCheckForDamage |= LayerMask.NameToLayer($"Destructible");
        return layerToCheckForDamage;
    }

    public float GetDamage(ActorHealth hitActor)
    {
        return Damage;
    }
}

public enum EntityType
{
    Enemy,
    Player
}