using UnityEngine;

public class AnimatePlayerEquipmentChanging : MonoBehaviour
{
    private PlayerRoot PlayerRoot { get; set; }
    private static readonly int WeaponLeft = Animator.StringToHash("WeaponLeft");
    private static readonly int WeaponRight = Animator.StringToHash("WeaponRight");
    private Animator Animator { get; set; }

    private void Start()
    {
        PlayerRoot = GetComponentInParent<PlayerRoot>();
        PlayerRoot.ActorEvents.OnEquippedWeaponChangedEvent += OnWeaponChanged;
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
    }

    private void OnWeaponChanged(object sender, OnEquippedWeaponChangedEventArgs args)
    {
        if (args.Slot == PlayerInventory.EquipmentSlot.LeftHand)
        {
            Animator.SetInteger(WeaponLeft, (int) args.NewValue);
        }
        else
        {
            Animator.SetInteger(WeaponRight, (int) args.NewValue);
        }
    }
}