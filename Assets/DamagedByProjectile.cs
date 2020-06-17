using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedByProjectile : MonoBehaviour
{
    [SerializeField] private AnimationMaterialHelper MaterialHelper;
    [SerializeField] private EnemyEvents Events;

    public bool OnShot(Vector2 textureCoord)
    {
        if (MaterialHelper.QueryAlpha(textureCoord))
        {
            Events.OnShot();
            return true;
        }

        return false;
    }
}