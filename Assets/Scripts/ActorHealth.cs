using System;
using UnityEngine;

public class ActorHealth : MonoBehaviour
{
    [SerializeField] private ActorEvents m_Events;
    public ActorEvents Events => m_Events;
    public bool IsAlive { get; set; } = true;

    [SerializeField] private float m_Health;
    [SerializeField] private float m_MaxHealth;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private Animator Animator { get; set; }

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
        Animator = GetComponentInChildren<Animator>();
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
        Health -= args.ProjectileInfo.GetDamage(this);
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