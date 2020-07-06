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

    public override void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
           Ray ray = new Ray(playerPosition, playerDirection * Range);
            Debug.DrawRay(playerPosition, playerDirection * Range, Color.blue, 10);
            LayerMask layerToCheck = ownerType == EntityType.Player
                ? LayerMask.GetMask("EnemyRaycast")
                : LayerMask.GetMask("Player");
            
            bool isDone = false;
            while (!isDone)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, Range, ~LayerMask.GetMask("Enemy")))
                {
                    var hitLayer = hit.collider.gameObject.layer;
                    if (hitLayer != layerToCheck)
                    {
                        isDone = true;
                    }

                    var hitCoord = hit.textureCoord;
                    // Debug.Log($"hit {hit.textureCoord} ");

                    IDamagedByHitscanProjectile damaged = hit.collider.GetComponent<IDamagedByHitscanProjectile>();
                    if (damaged != null && damaged.OnShot(hitCoord, this))
                    {
                        isDone = true;
                        var hitEffect = Instantiate(OnHitPrefab, damaged.transform, true);

                        float adjustmentDistance = .1f;

                        hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
                    }
                    else if (hitLayer != LayerMask.NameToLayer("Default"))
                    {
                        ray.origin = hit.point + (hit.normal * -.01f);
                    }
                    else if(OnHitWallPrefab)
                    {
                        var hitRotation = Quaternion.LookRotation(-hit.normal);
                        var hitEffect = Instantiate(OnHitWallPrefab, hit.collider.transform.position, hitRotation, hit.transform );

                        float adjustmentDistance = .001f;

                        hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
                        
                        // hitEffect.transform.localRotation = hitRotation;
                        isDone = true;
                    }
                    else
                    {
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