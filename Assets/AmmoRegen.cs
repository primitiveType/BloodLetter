using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AmmoRegen : MonoBehaviour
{
    private PlayerInventory Inventory;
    [SerializeField] private float RegenPerSecond;
    [SerializeField] private float SecondsWaitAfterUseAmmo;
    [SerializeField] private AmmoType TypeToRegen;

    private float secondsPerRegen;

    private float AmmoUseTimestamp;

    private void Start()
    {
        Inventory = Toolbox.Instance.PlayerInventory;
        Toolbox.Instance.PlayerEvents.OnAmmoChangedEvent += PlayerEventsOnOnAmmoChangedEvent;
        Inventory.GainAmmo(TypeToRegen, 1000);

        secondsPerRegen = 1f / RegenPerSecond;
    }

    private void PlayerEventsOnOnAmmoChangedEvent(object sender, OnAmmoChangedEventArgs args)
    {
        if (args.Type == TypeToRegen && args.OldValue > args.NewValue)
        {
            AmmoUseTimestamp = Time.time;
        }
    }

    private float timeAccumulated;

    private void Update()
    {
        if (Time.time > AmmoUseTimestamp + SecondsWaitAfterUseAmmo)
        {
            timeAccumulated += Time.deltaTime;
        }

        while (timeAccumulated >= secondsPerRegen)
        {
            Inventory.GainAmmo(TypeToRegen, 1);
            timeAccumulated -= secondsPerRegen;
        }
    }

    private void OnDestroy()
    {
        if (Toolbox.Instance?.PlayerEvents != null) 
        {
            Toolbox.Instance.PlayerEvents.OnAmmoChangedEvent -= PlayerEventsOnOnAmmoChangedEvent;
        }
    }
}