using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MonsterAttackComponent : MonoBehaviour
{
    [SerializeField] private ActorEvents m_Events;

    [SerializeField] private List<ProjectileInfoBase> m_Attacks;


    public float AttackCooldown => m_AttackCooldown;

    [SerializeField] private float m_AttackCooldown = 3f;
    private List<ProjectileInfoBase> Attacks { get; set; }

    private Animator Animator
    {
        get => m_animator;
        set => m_animator = value;
    }

    private Transform Target { get; set; }
    private Transform MonsterTransform { get; set; }

    private float LastAttackTimeStamp { get; set; }

    private static readonly int Attacking = Animator.StringToHash("Attacking");

    [SerializeField] private Animator m_animator;

    [SerializeField] private MonsterVisibilityHandler VisibilityHandler;

    private ProjectileInfoBase CurrentAttack { get; set; }

    public ActorEvents Events
    {
        get => m_Events;
        set => m_Events = value;
    }

    private void Start()
    {
        Attacks = m_Attacks.OrderBy(a => a.Range).ToList();
        Events.OnAttackEvent += OnEnemyAttack;
        Target = Toolbox.Instance.PlayerHeadTransform;
        MonsterTransform = transform;
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        Vector3 position = MonsterTransform.position;
        CurrentAttack.TriggerShoot(position, Target.position - position,
            EntityType.Enemy);
    }


    public void Update()
    {
        if (!(Time.time - LastAttackTimeStamp > AttackCooldown) || !VisibilityHandler.CanSeePlayer())
        {
            Animator.SetBool(Attacking, false);
            return;
        }

        var distance = Vector3.Distance(Target.position, transform.position);

        foreach (ProjectileInfoBase attack in Attacks)
        {
            if (distance <= attack.Range && distance >= attack.MinRange)
            {
                CurrentAttack = attack;
                Animator.SetBool(Attacking, true);
                LastAttackTimeStamp = Time.time;
                break;
            }
            else
            {
                Animator.SetBool(Attacking, false);
            }
        }
    }


    private void OnDestroy()
    {
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}