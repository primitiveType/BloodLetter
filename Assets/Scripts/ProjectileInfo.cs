﻿using UnityEngine;

public class ProjectileInfo : ProjectileInfoBase
{
    public GameObject ProjectilePrefab;

    public override void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType)
    {
        var projectile = GameObject.Instantiate(ProjectilePrefab);
        projectile.layer = ownerType == EntityType.Player
            ? LayerMask.NameToLayer("PlayerProjectile")
            : LayerMask.NameToLayer("EnemyProjectile");
        projectile.transform.position = playerPosition + (playerDirection * .25f);
        projectile.transform.forward = playerDirection;
    }

}