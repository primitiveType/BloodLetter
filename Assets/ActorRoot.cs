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
    public MonsterVisibilityHandler VisibilityHandler { get; private set; }
    public ActorHealth Health { get; private set; }
    public ActorArmor Armor { get; private set; }

    public virtual EntityType EntityType => EntityType.Enemy;
    
    

    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        ActorEvents = GetComponentInChildren<IActorEvents>();
        AnimationMaterialHelper = GetComponentInChildren<AnimationMaterialHelper>();
        HitscanCollider = GetComponentInChildren<IDamagedByHitscanProjectile>();
        AggroHandler = GetComponentInChildren<EnemyAggroHandler>();
        VisibilityHandler = GetComponentInChildren<MonsterVisibilityHandler>();
        Health = GetComponentInChildren<ActorHealth>();
        Armor = GetComponentInChildren<ActorArmor>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}