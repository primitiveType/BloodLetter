using UnityEngine;

public class TestUnequip : MonoBehaviour
{
    [SerializeField] private KeyCode EquipKey;
    [SerializeField] private bool equipOnStart;

    [SerializeField] private EquipStatus Equippable;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(EquipKey)) Toolbox.Instance.PlayerInventory.EquipThing(Equippable);
    }

    private void Start()
    {
        var savedWeapon = SaveState.Instance.SaveData.InventoryData.EquippedWeapon;
        if (savedWeapon != 0)
            equipOnStart = SaveState.Instance.SaveData.InventoryData.EquippedWeapon == Equippable.WeaponId;

        if (equipOnStart)
            Toolbox.Instance.PlayerInventory.EquipThing(Equippable);
        else
            Equippable.UnEquipInstant();
    }
}