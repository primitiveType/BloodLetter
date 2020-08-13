using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


public class MonsterAttackComponent : MonoBehaviour
{
    [SerializeField] private IActorEvents m_Events;

    [SerializeField] private List<ProjectileInfoBase> m_Attacks;


    public float AttackCooldown => m_AttackCooldown;

    [SerializeField] private float m_AttackCooldown = 3f;
    [SerializeField] private float m_AttackCooldownVariance = 1.5f;
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
    [SerializeField] private EnemyAggroHandler AggroHandler;

    private ProjectileInfoBase CurrentAttack { get; set; }

    private ActorRoot ActorRoot { get; set; }
    public IActorEvents Events => ActorRoot.ActorEvents;

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        Attacks = m_Attacks.OrderBy(a => a.Range).ToList();
        Events.OnAttackEvent += OnEnemyAttack;
        Target = Toolbox.Instance.PlayerHeadTransform;
        MonsterTransform = transform;
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        Vector3 position = MonsterTransform.position;
        CurrentAttack.TriggerShoot(MonsterTransform, Target.position - position, ActorRoot);
    }


    public void Update()
    {
        if (!(Time.time - LastAttackTimeStamp > AttackCooldown) || !VisibilityHandler.CanSeePlayer() ||
            !AggroHandler.IsAggro)
        {
            Animator.SetBool(Attacking, false);
            if (!AggroHandler.IsAggro)
            {
                ResetAttackTimeStamp(); //hack to add delay when aggro'd
            }

            return;
        }

        var distance = Vector3.Distance(Target.position, transform.position);

        foreach (ProjectileInfoBase attack in Attacks)
        {
            if (distance <= attack.Range && distance >= attack.MinRange)
            {
                CurrentAttack = attack;
                Animator.SetBool(Attacking, true);
                ResetAttackTimeStamp();
                break;
            }
            else
            {
                Animator.SetBool(Attacking, false);
            }
        }
    }

    private void ResetAttackTimeStamp()
    {
        LastAttackTimeStamp = Time.time + UnityEngine.Random.Range(0, m_AttackCooldownVariance);
    }


    private void OnDestroy()
    {
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}