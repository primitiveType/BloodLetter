using UnityEngine;

public class ProjectileInfo : ProjectileInfoBase
{
    [SerializeField] private bool parentProjectileToBarrel;
    public GameObject ProjectilePrefab;

    private GameObject CreateProjectile(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
        var projectile = Instantiate(gameObject);
        projectile.layer = ownerType == EntityType.Player
            ? LayerMask.NameToLayer("PlayerProjectile")
            : LayerMask.NameToLayer("EnemyProjectile");
        projectile.transform.position = playerPosition + playerDirection * .25f;
        projectile.transform.forward = playerDirection;
        return projectile;
    }

    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        GameObject proj = CreateProjectile(owner.position, direction, actorRoot.EntityType);
        proj.transform.SetParent(parentProjectileToBarrel ? owner : null);
        proj.SetActive(true);

        var exploder = proj.GetComponentInChildren<ExplodeOnCollide>();
        if (exploder)
        {
            exploder.SetIgnoreCollision(actorRoot.Collider.gameObject);
        }
    }
}