using System;

public interface IPostProcessHandle : IDisposable
{
    void SetWeight(float weight);
}