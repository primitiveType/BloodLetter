﻿using UnityEngine;

public class WeaponEventsHandler : MonoBehaviour
{
    [SerializeField] private Transform BarrelTransform;
    [SerializeField] private ShootDirectionProvider ShootDirectionProvider;
    [SerializeField] public ProjectileInfoBase ProjectileInfo;
    public WeaponSoundInfo SoundInfo;
    [SerializeField] private WeaponId WeaponId;


    private Vector3 ShootDirection => ShootDirectionProvider.ShootDirection;


    public void Shoot(WeaponId weaponId)
    {
        if (!WeaponId.HasFlag(weaponId))
        {
            return;
        }

        Debug.Log("shoot");

        bool didShoot =
            ProjectileInfo.TriggerShoot(BarrelTransform, ShootDirection, Toolbox.Instance.PlayerRoot,
                null); //null target for now.
        if (SoundInfo != null)
        {
            SoundInfo.OnShoot();
        }

        if (didShoot)//a bit of a hack. Just for the syringe. Might use for chainsaw as well.
            Toolbox.Instance.PlayerEvents.PlayerShoot(ProjectileInfo, WeaponId);
    }

    public void Idle()
    {
        SoundInfo.OnIdle();
    }

    public void Reload()
    {
        SoundInfo.OnReload();
    }

    private void OnEnable()
    {
    }
}