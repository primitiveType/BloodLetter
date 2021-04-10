using System;
using UnityEngine;

public class ActorHealth : MonoBehaviour
{
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private ActorRoot _actorRoot;
    [SerializeField] private Animator _animator;
    [SerializeField] private ActorArmor m_Armor;

    [SerializeField] private float m_Health;
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_OverhealMaxHealth;
    public IActorEvents Events => ActorRoot.ActorEvents;
    public bool IsAlive { get; set; } = true;

    private ActorRoot ActorRoot
    {
        get => _actorRoot != null ? _actorRoot : _actorRoot = GetComponentInParent<ActorRoot>();
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
            var diff = Math.Abs(m_Health - prevHealth);

            if (diff <= float.Epsilon) return;

            Events.OnHealthChanged(Mathf.Abs(diff), Health > prevHealth);
        }
    }

    public float MaxHealth
    {
        get => m_MaxHealth;
        set => m_MaxHealth = value;
    }

    public float OverhealMaxHealth
    {
        get => m_OverhealMaxHealth;
        set => m_OverhealMaxHealth = value;
    }


    public bool IsFullHealth => MaxHealth - Health <= float.Epsilon;
    public bool IsFullOverHealth => OverhealMaxHealth - Health <= float.Epsilon;

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            return;
        }

        EnemyData data = dataProvider.Data;

        MaxHealth = data.MaxHealth;
        Health = data.StartHealth;
        OverhealMaxHealth = data.OverhealMaxHealth;

    }

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        if (Animator == null) Animator = GetComponentInChildren<Animator>();

        UpdateAnimatorStates();
        Events.OnShotEvent += OnEnemyShot;
    }

    private void UpdateAnimatorStates()
    {
        if (Animator) Animator.SetBool(IsDead, !IsAlive);
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        var baseDamage = args.ProjectileInfo.GetDamage();
        var resultingDamage = baseDamage.Amount;
        if (m_Armor) resultingDamage = m_Armor.TakeDamage(baseDamage);

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

    public void Heal(float amount, bool canOverheal = false)
    {
        if (Health >= MaxHealth && !canOverheal) return;
        var newHealth = Health;
        newHealth = Mathf.Clamp(newHealth + amount, 0, canOverheal ? OverhealMaxHealth : MaxHealth);

        Health = newHealth;
        UpdateAnimatorStates();
    }
}