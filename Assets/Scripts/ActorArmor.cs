using System;
using UnityEngine;

public class ActorArmor : MonoBehaviour
{
    [SerializeField] private IActorEvents Events;
    [SerializeField] private float m_RemainingArmor;
    private readonly float reduc = .5f;

    public float MaxArmor { get; set; }
    public float OverhealMaxArmor { get; set; }

    public float CurrentArmor
    {
        get => m_RemainingArmor;
        set => m_RemainingArmor = value;
    }

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            return;
        }

        EnemyData data = dataProvider.Data;
        MaxArmor = data.MaxArmor;
        CurrentArmor = data.StartArmor;
        OverhealMaxArmor = data.OverhealMaxArmor;
    }

    private void Start()
    {
        Events = GetComponent<IActorEvents>();
    }

    public float TakeDamage(Damage baseDamage)
    {
        var amount = baseDamage.Amount;
        var invulnerable = GetComponent<Invulnerable>();
        if (invulnerable && invulnerable.DamageToIgnore.HasFlag(baseDamage.Type)) return 0;


        var prevValue = m_RemainingArmor;
        //var maxDamageBeforeReduction = m_RemainingArmor / reduc;
        var amountReduced = Mathf.Min(amount * reduc, m_RemainingArmor);
        m_RemainingArmor = Mathf.Clamp(m_RemainingArmor - amountReduced, 0, 100);

        if (prevValue != m_RemainingArmor) Events.OnArmorChanged();

        return amount - amountReduced;
    }

    public bool IsFull()
    {
        return m_RemainingArmor >= MaxArmor;
    }

    public void GainArmor(float amount, bool canOverheal)
    {
        var prevValue = m_RemainingArmor;
        m_RemainingArmor = Mathf.Clamp(m_RemainingArmor + amount, 0, canOverheal ? OverhealMaxArmor : MaxArmor);
        if (prevValue != m_RemainingArmor) Events.OnArmorChanged();
    }
}