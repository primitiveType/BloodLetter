public class OnKeysChangedEventArgs
{
    public OnKeysChangedEventArgs(KeyType oldValue, KeyType newValue)
    {
        NewValue = newValue;
        OldValue = oldValue;
    }

    public KeyType NewValue { get; }
    public KeyType OldValue { get; }
}