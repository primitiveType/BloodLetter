using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private AmmoType AmmoType;
    [SerializeField] private Text Text;

    private ActorEvents Events;
    private PlayerInventory Inventory;

    private bool IsDirty { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Inventory = Toolbox.PlayerInventory;
        Events = Toolbox.PlayerEvents;
        Events.OnAmmoChangedEvent += AmmoChanged;
        IsDirty = true;
    }

    private void AmmoChanged(object sender, OnAmmoChangedEventArgs onHealthChangedEventArgs)
    {
        IsDirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDirty)
        {
            Text.text = $"{Inventory.GetAmmoAmount(AmmoType)} / {Inventory.GetMaxAmmoAmount(AmmoType)}";
        }
    }

    private void OnDestroy()
    {
        Events.OnAmmoChangedEvent -= AmmoChanged;
    }
}