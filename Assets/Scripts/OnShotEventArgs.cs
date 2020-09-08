using UnityEngine;

public class OnShotEventArgs
{
    public OnShotEventArgs(IDamageSource projectileInfo, Vector3 worldPos)
    {
        ProjectileInfo = projectileInfo;
        WorldPos = worldPos;
    }

    public IDamageSource ProjectileInfo { get; }
    public Vector3 WorldPos { get; }
}