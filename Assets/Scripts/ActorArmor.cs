using System;
using UnityEngine;

public class ActorArmor : MonoBehaviour
{
    [SerializeField] private IActorEvents Events;
    [SerializeField] private float m_RemainingArmor;
    private float reduc = .5f;

    public float MaxArmor => 100;

    public float CurrentArmor => m_RemainingArmor;

    private void Start()
    {
        Events = GetComponent<IActorEvents>();
    }

    public float TakeDamage(float baseDamage)
    {
        var prevValue = m_RemainingArmor;
        //var maxDamageBeforeReduction = m_RemainingArmor / reduc;
        var amountReduced = Mathf.Min(baseDamage * reduc, m_RemainingArmor);
        m_RemainingArmor = Mathf.Clamp(m_RemainingArmor - amountReduced, 0, 100);

        if (prevValue != m_RemainingArmor)
        {
            Events.OnArmorChanged();
        }

        return baseDamage - amountReduced;
    }

    public bool IsFull()
    {
        return m_RemainingArmor >= MaxArmor;
    }

    public void GainArmor(float amount)
    {
        var prevValue = m_RemainingArmor;
        m_RemainingArmor = Mathf.Clamp(m_RemainingArmor + amount, 0, MaxArmor);
        if (prevValue != m_RemainingArmor)
        {
            Events.OnArmorChanged();
        }
    }
}