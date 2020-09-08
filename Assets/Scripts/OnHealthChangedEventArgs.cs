public class OnHealthChangedEventArgs
{
    public OnHealthChangedEventArgs(float amount, bool isHealing = false)
    {
        IsHealing = isHealing;
        Amount = amount;
    }

    public float Amount { get; }
    public bool IsHealing { get; }
}