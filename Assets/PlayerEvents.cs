using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public event PlayerShootEvent PlayerShootEvent;

    private void Awake()
    {
        Toolbox.SetPlayerEvents(this);
        Toolbox.SetPlayerTransform(transform);
    }

    public void PlayerShoot()
    {
        PlayerShootEvent?.Invoke(this, new PlayerShootEventArgs());
    }
}