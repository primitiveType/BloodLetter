using UnityEngine;

public class AmmoRegen : MonoBehaviour
{
    private float AmmoUseTimestamp;
    private PlayerInventory Inventory;
    [SerializeField] private float RegenPerSecond;

    private float secondsPerRegen;
    [SerializeField] private float SecondsWaitAfterUseAmmo;

    private float timeAccumulated;
    [SerializeField] private AmmoType TypeToRegen;

    private void Start()
    {
        Inventory = Toolbox.Instance.PlayerInventory;
        Toolbox.Instance.PlayerEvents.OnAmmoChangedEvent += PlayerEventsOnOnAmmoChangedEvent;

        secondsPerRegen = 1f / RegenPerSecond;
    }

    private void PlayerEventsOnOnAmmoChangedEvent(object sender, OnAmmoChangedEventArgs args)
    {
        if (args.Type == TypeToRegen && args.OldValue > args.NewValue) AmmoUseTimestamp = Time.time;
    }

    private void Update()
    {
        if (Time.time > AmmoUseTimestamp + SecondsWaitAfterUseAmmo) timeAccumulated += Time.deltaTime;

        while (timeAccumulated >= secondsPerRegen)
        {
            Inventory.GainAmmo(TypeToRegen, 1);
            timeAccumulated -= secondsPerRegen;
        }
    }

    private void OnDestroy()
    {
        if (Toolbox.Instance?.PlayerEvents != null)
            Toolbox.Instance.PlayerEvents.OnAmmoChangedEvent -= PlayerEventsOnOnAmmoChangedEvent;
    }
}