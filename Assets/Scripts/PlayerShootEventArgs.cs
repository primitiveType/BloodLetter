public class PlayerShootEventArgs
{
    public ProjectileInfoBase Info { get; }
    public WeaponId WeaponId { get; }

    public PlayerShootEventArgs(ProjectileInfoBase info, WeaponId weaponId)
    {
        Info = info;
        WeaponId = weaponId;
    }
}