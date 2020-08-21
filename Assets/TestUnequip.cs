using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnequip : MonoBehaviour
{
    [SerializeField] KeyCode EquipKey;

    [SerializeField] private EquipStatus Equippable;
    [SerializeField] private bool equipOnStart;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(EquipKey))
        {
            Toolbox.Instance.PlayerInventory.EquipThing(Equippable);
        }
    }

    void Start()
    {
        WeaponId savedWeapon = SaveState.Instance.SaveData.InventoryData.EquippedWeapon;
        if (savedWeapon != 0)
        {
            equipOnStart = (SaveState.Instance.SaveData.InventoryData.EquippedWeapon == Equippable.WeaponId);
        }

        if (equipOnStart)
        {
            Toolbox.Instance.PlayerInventory.EquipThing(Equippable);
        }
        else
        {
            Equippable.UnEquipInstant();
        }
    }


}