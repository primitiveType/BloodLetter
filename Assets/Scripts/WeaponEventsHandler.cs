using System;
using UnityEngine;

public class WeaponEventsHandler : MonoBehaviour
{
    [SerializeField]public ProjectileInfoBase ProjectileInfo;
    [SerializeField] private Transform BarrelTransform;
    public WeaponSoundInfo SoundInfo;
    private void Update()
    {
    }

    public void Shoot()
    {
        var transform1 = this.transform;
        ProjectileInfo.TriggerShoot(BarrelTransform.position, BarrelTransform.forward, EntityType.Player);
        SoundInfo.OnShoot();
        Toolbox.PlayerEvents.PlayerShoot();
    }

    public void Reload()
    {
        SoundInfo.OnReload();
    }
}