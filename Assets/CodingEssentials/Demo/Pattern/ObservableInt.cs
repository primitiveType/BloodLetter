using System;
using CodingEssentials.Pattern;

namespace CodingEssentials.Demo.Pattern
{
    /// <summary>
    ///     This class is necessary, because Unity can't handle generics directly.
    /// </summary>
    [Serializable]
    public class ObservableInt : UnityObservable<int, IntEvent>
    {
    }
}