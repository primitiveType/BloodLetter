﻿using System.Collections.Generic;
using UnityEngine;

public class ChainHitscanProjectileInfo : HitscanProjectileInfo
{
    Collider[] HitsArray = new Collider[20];

    [SerializeField] private float m_Radius;
    [SerializeField] private LineRenderer m_Visual; 

    public override void TriggerShoot(Transform owner, Vector3 ownerDirection, ActorRoot actorRoot)
    {
        List<IDamagedByHitscanProjectile> alreadyHit = new List<IDamagedByHitscanProjectile>();
        ownerRoot = actorRoot;
        IDamagedByHitscanProjectile damaged = HitscanUtils.GetHitObject(owner.position, ownerDirection, actorRoot,
            Range,
            OnHitWallPrefab,
            out RaycastHit hit);

        if (damaged == null)
        {
            return;
        }

        damaged.OnShot(hit.point, this, ownerDirection);
        alreadyHit.Add(damaged);
        ChainShoot(damaged.transform, actorRoot, alreadyHit, damaged.transform.position);
    }

    private void ChainShoot(Transform owner, ActorRoot actorRoot,
        List<IDamagedByHitscanProjectile> alreadyHit, Vector3 overlapPosition)
    {
     
        var victims =
            HitscanUtils.CheckOverlap(overlapPosition, actorRoot, HitsArray, m_Radius);
        foreach (IDamagedByHitscanProjectile victim in victims)
        {
            if (!alreadyHit.Contains(victim) || !victim.ActorRoot.Health.IsAlive)
            {
                alreadyHit.Add(victim);
                var line = Instantiate(m_Visual);
                Vector3 position = victim.ActorRoot.VisibilityHandler.transform.position;//more likely to be the head of creature
                line.SetPositions(new Vector3[2]{overlapPosition, position});
                victim.OnShot(this, position, victim.transform.forward);
                ChainShoot(owner,
                    actorRoot, alreadyHit, position);
            }
        }
    }
}