using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class HitscanProjectileInfo : ProjectileInfoBase
{
    public bool Piercing;
    public GameObject OnHitPrefab;
    public GameObject OnHitWallPrefab;
    [SerializeField] private float m_Damage;
    public float Damage => m_Damage;

    public override void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
           Ray ray = new Ray(playerPosition, playerDirection * 100);
            Debug.DrawRay(playerPosition, playerDirection * 100, Color.blue, 10);

            bool isDone = false;
            while (!isDone)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 1000, ~LayerMask.GetMask("Enemy")))
                {
                    var hitLayer = hit.collider.gameObject.layer;
                    if (hitLayer != LayerMask.NameToLayer("EnemyRaycast"))
                    {
                        isDone = true;
                    }

                    var hitCoord = hit.textureCoord;
                    // Debug.Log($"hit {hit.textureCoord} ");

                    DamagedByProjectile damaged = hit.collider.GetComponent<DamagedByProjectile>();
                    if (damaged && damaged.OnShot(hitCoord, this))
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
                    else
                    {
                        var hitRotation = Quaternion.LookRotation(-hit.normal);
                        var hitEffect = Instantiate(OnHitWallPrefab, hit.collider.transform.position, hitRotation, hit.transform );

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
}

public enum EntityType
{
    Enemy,
    Player
}