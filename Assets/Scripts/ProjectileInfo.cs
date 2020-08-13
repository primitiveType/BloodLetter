using UnityEngine;

public class ProjectileInfo : ProjectileInfoBase
{
    public GameObject ProjectilePrefab;

    public override void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
        CreateProjectile(playerPosition, playerDirection, ownerType);
    }

    private GameObject CreateProjectile(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
        var projectile = GameObject.Instantiate(ProjectilePrefab);
        projectile.layer = ownerType == EntityType.Player
            ? LayerMask.NameToLayer("PlayerProjectile")
            : LayerMask.NameToLayer("EnemyProjectile");
        projectile.transform.position = playerPosition + (playerDirection * .25f);
        projectile.transform.forward = playerDirection;
        return projectile;
    }

    public override void TriggerShoot(Transform owner, Vector3 direction, EntityType ownerType, ActorRoot actorRoot)
    {
        var proj = CreateProjectile(owner.position, direction, ownerType);
        proj.transform.SetParent(owner);
    }
}