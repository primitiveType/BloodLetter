using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ProjectileInfo : MonoBehaviour
{
    public bool Hitscan;
    public bool Piercing;
    public GameObject OnHitPrefab;
    public GameObject ProjectilePrefab;

    public void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection)
    {
        if (Hitscan)
        {
            Debug.Log("shoot");
            Ray ray = new Ray(playerPosition, playerDirection * 100);
            Debug.DrawRay(playerPosition, playerDirection * 100, Color.blue, 10);

            bool isDone = false;
            while (!isDone)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Enemy"),
                    QueryTriggerInteraction.Collide))
                {
                    var hitCoord = hit.textureCoord;
                    // Debug.Log($"hit {hit.textureCoord} ");

                    DamagedByProjectile damaged = hit.collider.GetComponent<DamagedByProjectile>();
                    if (damaged && damaged.OnShot(hitCoord))
                    {
                        isDone = true;
                        var hitEffect = Instantiate(OnHitPrefab, damaged.transform, true);

                        float adjustmentDistance = -.01f;

                        hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
                    }
                    else
                    {
                        ray.origin = hit.point + (hit.normal * .01f);
                    }
                }
            }
        }
    }
}