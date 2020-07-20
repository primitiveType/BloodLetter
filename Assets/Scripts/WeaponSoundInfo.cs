using System;
using UnityEngine;

public class WeaponSoundInfo : MonoBehaviour
{
    [SerializeField] private bool LoopShootSound;
    [SerializeField] private bool LoopIdleSound;
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip ReloadSound;
    [SerializeField] private AudioClip IdleSound;

    [SerializeField] private AudioSource Source;

    private EquipStatus EquipStatus { get; set; }

    private void Start()
    {
        EquipStatus = GetComponent<EquipStatus>();
        Toolbox.Instance.PlayerEvents.OnEquippedWeaponChangedEvent += PlayerEventsOnOnWeaponsChangedEvent;
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerEvents.OnEquippedWeaponChangedEvent -= PlayerEventsOnOnWeaponsChangedEvent;
    }

    private void PlayerEventsOnOnWeaponsChangedEvent(object sender, OnEquippedWeaponChangedEventArgs args)
    {
        Source.enabled = (args.NewValue & EquipStatus.WeaponId) != 0;
    }

    public void OnShoot()
    {
        if (ShootSound && LoopShootSound)
        {
            Source.clip = ShootSound;
            Source.loop = true;
            if (!Source.isPlaying)
            {
                Source.Play();
            }
        }
        else
        {
            Source.PlayOneShot(ShootSound);
            Source.loop = false;
        }
    }

    public void OnReload()
    {
        Source.PlayOneShot(ReloadSound);
    }

    public void OnIdle()
    {
        if (!Source.enabled)
        {
            return;
        }
        if (IdleSound && LoopIdleSound)
        {
            Source.clip = IdleSound;
            Source.loop = true;
            if (!Source.isPlaying)
            {
                Source.Play();
            }
        }
        else
        {
            Source.PlayOneShot(IdleSound);
            Source.loop = false;
        }
    }
}