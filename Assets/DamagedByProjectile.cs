using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedByProjectile : MonoBehaviour
{
    [SerializeField] private AnimationMaterialHelper MaterialHelper;
    [SerializeField] private EnemyEvents Events;

    public bool OnShot(Vector2 textureCoord, ProjectileInfo projectileInfo)
    {
        if (MaterialHelper.QueryAlpha(textureCoord))
        {
            Events.OnShot(projectileInfo);
            return true;
        }

        return false;
    }
}