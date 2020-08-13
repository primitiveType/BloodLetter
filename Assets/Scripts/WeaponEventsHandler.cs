using System;
using UnityEngine;

public class WeaponEventsHandler : MonoBehaviour
{
    [SerializeField] public ProjectileInfoBase ProjectileInfo;
    [SerializeField] private Transform BarrelTransform;
    public WeaponSoundInfo SoundInfo;

    private void Update()
    {
    }

    public void Shoot()
    {
        ProjectileInfo.TriggerShoot(BarrelTransform, BarrelTransform.forward, EntityType.Player);
        SoundInfo.OnShoot();
        Toolbox.Instance.PlayerEvents.PlayerShoot();
    }

    public void Idle()
    {
        SoundInfo.OnIdle();
    }

    public void Reload()
    {
        SoundInfo.OnReload();
    }

    void OnEnable()
    {
        Debug.Log("Enabled    ");
    }
}