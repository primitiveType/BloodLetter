using UnityEngine;

public class ActorArmor : MonoBehaviour
{
    [SerializeField] private float m_RemainingArmor;
    private float reduc = .5f;

    private float MaxArmor => 100;
    public float TakeDamage(float baseDamage)
    {
        //var maxDamageBeforeReduction = m_RemainingArmor / reduc;
        var amountReduced = Mathf.Min(baseDamage * reduc, m_RemainingArmor);
        m_RemainingArmor = Mathf.Clamp(m_RemainingArmor - amountReduced, 0, 100);

        return baseDamage - amountReduced;
    }

    public bool IsFull()
    {
        return m_RemainingArmor >= MaxArmor;
    }

    public void GainArmor(float amount)
    {
        m_RemainingArmor = Mathf.Clamp(m_RemainingArmor + amount, 0, MaxArmor);
    }
    
    
}