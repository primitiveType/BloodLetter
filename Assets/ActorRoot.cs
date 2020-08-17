using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ActorRoot : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public IActorEvents ActorEvents { get; private set; }

    public AnimationMaterialHelper AnimationMaterialHelper { get; private set; }
    public IDamagedByHitscanProjectile HitscanCollider { get; private set; }

    public EnemyAggroHandler AggroHandler { get; private set; }

    public virtual EntityType EntityType => EntityType.Enemy;

    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        ActorEvents = GetComponentInChildren<IActorEvents>();
        AnimationMaterialHelper = GetComponentInChildren<AnimationMaterialHelper>();
        HitscanCollider = GetComponentInChildren<IDamagedByHitscanProjectile>();
        AggroHandler = GetComponentInChildren<EnemyAggroHandler>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}