using UnityEngine;

public class WeaponEventsHandler : MonoBehaviour
{
    [SerializeField] private Transform BarrelTransform;
    [SerializeField] public ProjectileInfoBase ProjectileInfo;
    public WeaponSoundInfo SoundInfo;

    public void Shoot()
    {
        ProjectileInfo.TriggerShoot(BarrelTransform, BarrelTransform.forward, Toolbox.Instance.PlayerRoot);
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

    private void OnEnable()
    {
    }
}