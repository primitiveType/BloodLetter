using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArmorText : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;

  
    [SerializeField]
    private ActorArmor Armor;

    [SerializeField] private Text Text;

    private bool IsDirty { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Events.OnArmorChangedEvent += ArmorChanged;
        IsDirty = true;
    }

    private void ArmorChanged(object sender, OnArmorChangedEventArgs onArmorChangedEventArgs)
    {
        IsDirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDirty)
        {
            Text.text = $"{Armor.CurrentArmor} / {Armor.MaxArmor}";
        }
    }

    private void OnDestroy()
    {
        Events.OnArmorChangedEvent -= ArmorChanged;
    }
}