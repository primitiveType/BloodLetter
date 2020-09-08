using System;

namespace CodingEssentials.Pattern
{
    /// <summary>
    ///     Exception which is thrown if you try to instantiate multiple instances of the same singleton.
    /// </summary>
    [Serializable]
    public class SingletonException : Exception
    {
    }
}