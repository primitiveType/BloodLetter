namespace CodingEssentials
{
    /// <summary>
    ///     Internally used enum, which represents the actual state of a tribool.
    /// </summary>
    public enum TriboolState : sbyte
    {
        False = -1,
        Unknown = 0,
        True = 1
    }
}