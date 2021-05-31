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
        Events.OnDeathEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath(object sender, OnDeathEventArgs args)
    {
        enabled = false;
        Shaker.ResetTrauma();
    }

    private void OnPlayerHealthChanged(object sender, OnHealthChangedEventArgs args)
    {
        float damageToMaxTrauma = 20;
        if (!args.IsHealing && !Toolbox.Instance.IsPlayerDead)
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
        Shaker.AddTrauma(new Vector3(force, force * yzMultiplier, force * yzMultiplier));
    }

    private void OnPlayerShot(object sender, OnShotEventArgs args)
    {
        if (!Toolbox.Instance.IsPlayerDead)
        {
            Shaker.AddTrauma(args.ProjectileInfo.Force / 1000f);
        }
    }
    
}