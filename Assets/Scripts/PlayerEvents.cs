public class PlayerEvents : ActorEvents
{
    public event PlayerShootEvent PlayerShootEvent;
    public event PlayerGainBloodEvent PlayerGainBloodEvent;
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

    public void OnBloodGained()
    {
        PlayerGainBloodEvent?.Invoke(this, new PlayerGainBloodEventArgs());
    }
}

public delegate void PlayerGainBloodEvent(object sender, PlayerGainBloodEventArgs args);

public class PlayerGainBloodEventArgs
{
}