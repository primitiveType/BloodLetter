﻿using UnityEngine;

public abstract class ProjectileInfoBase : MonoBehaviour
{
    [SerializeField] private float m_Range = 100;
    [SerializeField] private float m_MinRange = 10;

    public float MinRange => m_MinRange;
    public float Range => m_Range;
    public abstract void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType);
    public abstract void TriggerShoot(Transform owner, Vector3 direction, EntityType ownerType, ActorRoot actorRoot);
}