using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Quaternion = UnityEngine.Quaternion;
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
        m_EnvironmentLayers != null ? m_EnvironmentLayers : m_EnvironmentLayers = new List<int>()
    {
        LayerMask.NameToLayer("Default"),
        LayerMask.NameToLayer("Interactable")
    };

    public override void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
           Ray ray = new Ray(playerPosition, playerDirection * Range);
            Debug.DrawRay(playerPosition, playerDirection * Range, Color.blue, 10);
            int layerToCheckForDamage = ownerType == EntityType.Player
                ? LayerMask.NameToLayer("EnemyRaycast")
                : LayerMask.NameToLayer("Player");

            layerToCheckForDamage |= LayerMask.NameToLayer($"Destructible");

            //always ignore this one, since its the enemy collider which we don't use for raycasts
            int raycastMask = ~LayerMask.GetMask("Enemy");
            
            bool isDone = false;
            while (!isDone)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, Range, raycastMask ))
                {
                    int hitLayer = hit.collider.gameObject.layer;
                    if (((hitLayer & layerToCheckForDamage) == 0) || hit.transform == null)
                    {
                        isDone = true;
                    }

                    var hitCoord = hit.textureCoord;
                    // Debug.Log($"hit {hit.textureCoord} ");

                    IDamagedByHitscanProjectile damaged = hit.collider.GetComponent<IDamagedByHitscanProjectile>();
                    if (damaged != null && damaged.OnShot(hitCoord, this))
                    {
                        isDone = true;
                        var hitEffect = GameObject.Instantiate(OnHitPrefab, damaged.transform, true);

                        float adjustmentDistance = .1f;

                        hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
                    }
                    else if(!EnvironmentLayers.Contains(hitLayer))
                    {
                        ray.origin = hit.point + (hit.normal * -.01f);
                    }
                    else if(OnHitWallPrefab)
                    {
                        var hitRotation = Quaternion.LookRotation(-hit.normal);
                        var hitEffect = GameObject.Instantiate(OnHitWallPrefab, hit.collider.transform.position, hitRotation, hit.transform );

                        float adjustmentDistance = .001f;

                        hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
                        
                        // hitEffect.transform.localRotation = hitRotation;
                        isDone = true;
                    }
                }
                else
                {
                    isDone = true;
                }
            }
        
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