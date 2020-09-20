using UnityEngine;

public class OnStepEventArgs
{
    public Vector3? LastPosition { get; }
    public Vector3? NewPosition { get; }

    public OnStepEventArgs(Vector3? lastPosition, Vector3? newPosition)
    {
        LastPosition = lastPosition;
        NewPosition = newPosition;
    }
}