using UnityEngine;

public class OnShotEventArgs
{
    public OnShotEventArgs(IDamageSource projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        ProjectileInfo = projectileInfo;
        WorldPos = worldPos;
        HitNormal = hitNormal;
    }

    public IDamageSource ProjectileInfo { get; }
    public Vector3 WorldPos { get; }
    public Vector3 HitNormal { get; }
}