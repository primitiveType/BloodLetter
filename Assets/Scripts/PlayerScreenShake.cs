using System;
using UnityEngine;

public class PlayerScreenShake : MonoBehaviour
{
    [SerializeField] private ScreenShake m_Shaker;
    private ScreenShake Shaker => m_Shaker;

    private PlayerEvents Events { get; set; }

    private void Start()
    {
        Events = Toolbox.Instance.PlayerEvents;
        Events.PlayerShootEvent += OnPlayerShoot;
        Events.OnShotEvent += OnPlayerShot;
        Events.OnHealthChangedEvent += OnPlayerHealthChanged;
    }

    private void OnPlayerHealthChanged(object sender, OnHealthChangedEventArgs args)
    {
        float damageToMaxTrauma = 20;
        if (!args.IsHealing)
        {
            Shaker.AddTrauma(args.Amount / damageToMaxTrauma);
        }
    }

    private void OnDestroy()
    {
        Events.PlayerShootEvent -= OnPlayerShoot;
        Events.OnShotEvent -= OnPlayerShot;
        Events.OnHealthChangedEvent -= OnPlayerHealthChanged;
    }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs args)
    {
        var force = args.Info.Force / 2000f;
        var yzMultiplier = .5f;
        Shaker.AddTrauma(new Vector3(force, force*yzMultiplier, force*yzMultiplier));
    }

    private void OnPlayerShot(object sender, OnShotEventArgs args)
    {
        Shaker.AddTrauma(args.ProjectileInfo.Force / 1000f);
    }

    private void OnScreenShakeSettingsChanged(float maxAngle)
    {
        // Shaker.MaxAngle = maxAngle;
    }
}