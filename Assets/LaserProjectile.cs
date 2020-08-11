using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserProjectile : HitscanProjectileInfo
{
    [SerializeField] private LineRenderer m_Line;
    [SerializeField] private GameObject HitPoint;

    public override void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, EntityType ownerType)
    {
        OwnerType = ownerType;
    }

    public EntityType OwnerType { get; set; } = EntityType.Enemy;

    private void FixedUpdate()
    {
        var position = transform.position;
        var damaged = GetHitObject(position, transform.forward, OwnerType, out RaycastHit hit);

        bool gotHit = hit.transform;

        m_Line.enabled = gotHit;
        HitPoint.SetActive(gotHit);
        HitPoint.transform.position = hit.point;
        m_Line.SetPositions(new Vector3[] {position, hit.point});
    }
}