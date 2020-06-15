using System;
using UnityEngine;

public class WeaponEventsHandler : MonoBehaviour
{
    public ProjectileInfo ProjectileInfo;

    public WeaponSoundInfo SoundInfo;
    private void Update()
    {
    }

    public void Shoot()
    {
        var transform1 = this.transform;
        ProjectileInfo.TriggerShoot(transform1.position, transform1.forward);
        SoundInfo.OnShoot();
    }

    public void Reload()
    {
        SoundInfo.OnReload();

    }
}