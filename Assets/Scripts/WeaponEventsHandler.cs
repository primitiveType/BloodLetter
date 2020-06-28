using System;
using UnityEngine;

public class WeaponEventsHandler : MonoBehaviour
{
    [SerializeField]public ProjectileInfoBase ProjectileInfo;

    public WeaponSoundInfo SoundInfo;
    private void Update()
    {
    }

    public void Shoot()
    {
        var transform1 = this.transform;
        ProjectileInfo.TriggerShoot(transform1.position, transform1.forward, EntityType.Player);
        SoundInfo.OnShoot();
        Toolbox.PlayerEvents.PlayerShoot();
    }

    public void Reload()
    {
        SoundInfo.OnReload();
    }
}