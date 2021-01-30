using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterAttackComponent : MonoBehaviour
{
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    [SerializeField] private EnemyAggroHandler AggroHandler;

    [SerializeField] private bool CooldownStartsOnAggro = true;

    [SerializeField] private Animator m_animator;

    [SerializeField] private float m_AttackCooldown = 3f;
    [SerializeField] private float m_AttackCooldownVariance = 1.5f;

    [SerializeField] private List<ProjectileInfoBase> m_Attacks;
    [SerializeField] private IActorEvents m_Events;

    [SerializeField] private MonsterVisibilityHandler VisibilityHandler;

    private Coroutine AttackRoutine;
    private readonly float attackTime = .5f;

    public bool IsAttacking { get; private set; }


    public float AttackCooldown => m_AttackCooldown;
    private List<ProjectileInfoBase> Attacks { get; set; }

    private Animator Animator
    {
        get => m_animator;
        set => m_animator = value;
    }

    private Transform Target { get; set; }
    private Transform AttackSourceTransform { get; set; }

    private float LastAttackTimeStamp { get; set; } = float.NegativeInfinity;

    private ProjectileInfoBase CurrentAttack { get; set; }

    private ActorRoot ActorRoot { get; set; }
    public IActorEvents Events => ActorRoot.ActorEvents;

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        Attacks = m_Attacks.OrderBy(a => a.Range).ToList();
        Events.OnAttackEvent += OnEnemyAttack;
        Target = Toolbox.Instance.PlayerHeadTransform;
        AttackSourceTransform = transform;
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        var position = AttackSourceTransform.position;
        var targetPosition = Target.position;
        CurrentAttack.TriggerShoot(AttackSourceTransform, targetPosition - position, ActorRoot);

        if (AttackRoutine != null) StopCoroutine(AttackRoutine);

        AttackRoutine = StartCoroutine(AttackTimerCr());
    }

    private IEnumerator
        AttackTimerCr() 
    {
        IsAttacking = true;
        AnimatorStateInfo currentAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);

        var length = currentAnimatorStateInfo.length * (1f - currentAnimatorStateInfo.normalizedTime);
        float startTime = Time.time;
        float time = startTime;
        yield return new WaitForEndOfFrame();
        while (time < startTime + length)
        {
            yield return new WaitForEndOfFrame();
            var position = ActorRoot.transform.position;
            var targetPosition = Target.position;
            var lookPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);
            ActorRoot.transform.LookAt(lookPosition);
            time += Time.deltaTime;
        }

        IsAttacking = false;
    }


    public void Update()
    {
        if (!(Time.time - LastAttackTimeStamp > AttackCooldown) || !VisibilityHandler.CanSeePlayer() ||
            !AggroHandler.IsAggro)
        {
            Animator.SetBool(Attacking, false);
            if (!AggroHandler.IsAggro && CooldownStartsOnAggro) ResetAttackTimeStamp(); //hack to add delay when aggro'd

            return;
        }

        var distance = Vector3.Distance(Target.position, transform.position);

        var validAttacks = Attacks.Where(a => distance <= a.Range && distance >= a.MinRange).ToList();
        var count = validAttacks.Count;
        if (count == 0)
        {
            Animator.SetBool(Attacking, false);
            return;
        }

        var random = Random.Range(0, count);
        CurrentAttack = validAttacks[random];
        Animator.SetBool(Attacking, true);
        ResetAttackTimeStamp();
    }

    private void ResetAttackTimeStamp()
    {
        LastAttackTimeStamp = Time.time + Random.Range(0, m_AttackCooldownVariance);
    }


    private void OnDestroy()
    {
        Events.OnAttackEvent -= OnEnemyAttack;
    }
}