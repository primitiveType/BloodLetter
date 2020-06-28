using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerEvents : ActorEvents
{
    public event PlayerShootEvent PlayerShootEvent;
    public event PlayerInteractEvent PlayerInteractEvent;

    private void Awake()
    {
        Toolbox.SetPlayerEvents(this);
        Toolbox.SetPlayerTransform(transform);
    }

    public void PlayerShoot()
    {
        PlayerShootEvent?.Invoke(this, new PlayerShootEventArgs());
    }

    public void PlayerInteract(IInteractable target)
    {
        PlayerInteractEvent?.Invoke(this, new PlayerInteractEventArgs(target));
    }
}

public delegate void PlayerInteractEvent(object sender, PlayerInteractEventArgs args);

public class PlayerInteractEventArgs
{
    public IInteractable Target { get; }

    public PlayerInteractEventArgs(IInteractable target)
    {
        Target = target;
    }
}