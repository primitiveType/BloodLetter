public class Key : IKey
{
    public Key(KeyType keyType)
    {
        KeyType = keyType;
    }

    public KeyType KeyType { get; }
}