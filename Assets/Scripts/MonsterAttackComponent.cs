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
    private Transform TargetCollider { get; set; }
    private Transform MonsterTransform { get; set; }

    private float LastAttackTimeStamp { get; set; }

    [SerializeField] private float AttackCooldown = 3f;
    private static readonly int Attacking = Animator.StringToHash("Attacking");

    [SerializeField] private ProjectileInfoBase AttackProjectile;
    [SerializeField] private Animator m_animator;

    public ActorEvents Events
    {
        get => m_Events;
        set => m_Events = value;
    }

    private void Start()
    {
        Events.OnAttackEvent += OnEnemyAttack;
        Target = Toolbox.PlayerHeadTransform;
        TargetCollider = Toolbox.PlayerTransform;
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
        if (!(Time.time - LastAttackTimeStamp > AttackCooldown) || !CanSeePlayer())
        {
            Animator.SetBool(Attacking, false);
            return;
        }

        Animator.SetBool(Attacking, true);
        LastAttackTimeStamp = Time.time;
    }

    private bool CanSeePlayer()
    {
        var angle = Vector3.Dot(Target.position - MonsterTransform.position, MonsterTransform.forward);
        if (angle < 0) //if monster isn't facing player
        {
            return false;
        }

        //might want to offset monster position so they can see over low walls, etc.
        Ray ray = new Ray(MonsterTransform.position, Target.position - MonsterTransform.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, LayerMask.GetMask("Player", "Default")))
        {
            return hitInfo.transform == TargetCollider;
        }


        return false;
    }

    private void OnDestroy()
    {
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}