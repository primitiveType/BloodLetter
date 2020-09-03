using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private AmmoType AmmoType;
    [SerializeField] private Text Text;

    private IActorEvents Events;
    private PlayerInventory Inventory;

    private bool IsDirty { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Inventory = Toolbox.Instance.PlayerInventory;
        Events = Toolbox.Instance.PlayerEvents;
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
            Text.text = $"{AmmoType} : {Mathf.FloorToInt(Inventory.GetAmmoAmount(AmmoType))} / {Mathf.FloorToInt(Inventory.GetMaxAmmoAmount(AmmoType))}";
            IsDirty = false;
        }
    }

    private void OnDestroy()
    {
        Events.OnAmmoChangedEvent -= AmmoChanged;
    }
}