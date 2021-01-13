public class PlayerEvents : ActorEvents
{
    public event PlayerShootEvent PlayerShootEvent;
    public event PlayerInteractEvent PlayerInteractEvent;

    private void Awake()
    {
        Toolbox.Instance.SetPlayerEvents(this);
        Toolbox.Instance.SetPlayerTransform(transform);
    }

    public void PlayerShoot(ProjectileInfoBase info)
    {
        PlayerShootEvent?.Invoke(this, new PlayerShootEventArgs(info));
    }

    public void PlayerInteract(IInteractable target)
    {
        PlayerInteractEvent?.Invoke(this, new PlayerInteractEventArgs(target));
    }
}