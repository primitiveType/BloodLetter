using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedByProjectile : MonoBehaviour
{
    [SerializeField] private AnimationMaterialHelper MaterialHelper;

    public bool OnShot(Vector2 textureCoord)
    {
        return MaterialHelper.QueryAlpha(textureCoord);
    }
}