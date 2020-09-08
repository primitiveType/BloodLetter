using System;
using UnityEngine.Events;

namespace CodingEssentials.Demo.Pattern
{
    /// <summary>
    ///     This class is necessary, because Unity can't handle generics directly.
    /// </summary>
    [Serializable]
    public class IntEvent : UnityEvent<int>
    {
    }
}