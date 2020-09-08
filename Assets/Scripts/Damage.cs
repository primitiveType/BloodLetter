public struct Damage
{
    public float Amount { get; set; }
    public DamageType Type { get; set; }

    public Damage(float amount, DamageType type)
    {
        Amount = amount;
        Type = type;
    }
}