using System;
using UnityEngine.Profiling;

public class ProfilerSample : IDisposable
{
    public ProfilerSample(string name)
    {
        Profiler.BeginSample(name);
    }

    public void Dispose()
    {
        Profiler.EndSample();
    }
}