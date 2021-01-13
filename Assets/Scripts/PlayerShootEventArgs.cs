public class PlayerShootEventArgs
{
    public ProjectileInfoBase Info { get; }

    public PlayerShootEventArgs(ProjectileInfoBase info)
    {
        Info = info;
    }
}