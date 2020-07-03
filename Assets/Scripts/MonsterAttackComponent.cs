using System;
using UnityEngine;

public class MonsterAttackComponent : MonoBehaviour
{
    [SerializeField] private ActorEvents m_Events;

    private Animator Animator
    {
        get => m_animator;
        set => m_animator = value;
    }

    private Transform Target { get; set; }
    private Transform MonsterTransform { get; set; }

    private float LastAttackTimeStamp { get; set; }

    [SerializeField] private float AttackCooldown = 3f;
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    [SerializeField] private ProjectileInfoBase AttackProjectile;
    [SerializeField] private Animator m_animator;

    [SerializeField] private MonsterVisibilityHandler VisibilityHandler;

    public ActorEvents Events
    {
        get => m_Events;
        set => m_Events = value;
    }

    private void Start()
    {
        Events.OnAttackEvent += OnEnemyAttack;
        Target = Toolbox.PlayerHeadTransform;
        MonsterTransform = transform;
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        Vector3 position = MonsterTransform.position;
        AttackProjectile.TriggerShoot(position, Target.position - position,
            EntityType.Enemy);
    }


    public void Update()
    {
        if (!(Time.time - LastAttackTimeStamp > AttackCooldown) || !VisibilityHandler.CanSeePlayer())
        {
            Animator.SetBool(Attacking, false);
            return;
        }

        Animator.SetBool(Attacking, true);
        LastAttackTimeStamp = Time.time;
    }



    private void OnDestroy()
    {
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}