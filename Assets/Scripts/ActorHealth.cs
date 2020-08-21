using System;
using UnityEngine;
using UnityEngine.Networking.Match;

public class ActorHealth : MonoBehaviour
{
    public IActorEvents Events => ActorRoot.ActorEvents;
    public bool IsAlive { get; set; } = true;

    [SerializeField] private float m_Health;
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private ActorArmor m_Armor;
    [SerializeField] private Animator _animator;
    private ActorRoot _actorRoot;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private ActorRoot ActorRoot
    {
        get => _actorRoot != null ? _actorRoot  : (_actorRoot = GetComponentInParent<ActorRoot>());
        set => _actorRoot = value;
    }

    private Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }

    public float Health
    {
        get => m_Health;
        set
        {
            var prevHealth = m_Health;
            m_Health = value;
            if (m_Health <= 0 && IsAlive) Die();

            if (m_Health > MaxHealth)
            {
                m_Health = MaxHealth;
            }

            var diff = Math.Abs(m_Health - prevHealth);

            if (diff <= float.Epsilon)
            {
                return;
            }

            Events.OnHealthChanged(Mathf.Abs(diff), Health > prevHealth);
        }
    }

    public float MaxHealth
    {
        get => m_MaxHealth;
        set => m_MaxHealth = value;
    }

    public bool IsFullHealth => Math.Abs(MaxHealth - Health) <= float.Epsilon;


    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        if (Animator == null)
        {
            Animator = GetComponentInChildren<Animator>();
        }

        UpdateAnimatorStates();
        Events.OnShotEvent += OnEnemyShot;
    }

    private void UpdateAnimatorStates()
    {
        if (Animator)
        {
            Animator.SetBool(IsDead, !IsAlive);
        }
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        var baseDamage = args.ProjectileInfo.GetDamage(this);
        var resultingDamage = baseDamage;
        if (m_Armor)
        {
            resultingDamage = m_Armor.TakeDamage(baseDamage);
        }

        Health -= resultingDamage;
        UpdateAnimatorStates();
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= OnEnemyShot;
    }

    private void Die()
    {
        IsAlive = false;
        UpdateAnimatorStates();
        Events.OnDeath();
    }

    public void Heal(float amount)
    {
        Health += amount;
        UpdateAnimatorStates();
    }
}