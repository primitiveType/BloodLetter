using UnityEngine;

internal class PlayerBlood : MonoBehaviour
{
    private float m_blood;
    private MovementModifierHandle Handle { get; } = new MovementModifierHandle();

    public bool BloodGivesMovementSpeed = false;

    private ActorRoot Root { get; set; }
    private void Start()
    {
        if (BloodGivesMovementSpeed)
        {
            GetComponent<IMovementHandler>().AddMovementModifier(Handle);
        }

        Root = GetComponent<PlayerRoot>();
        Toolbox.Instance.PlayerEvents.PlayerGainBloodEvent += OnBloodGained;
    }

    private void OnBloodGained(object sender, PlayerGainBloodEventArgs args)
    {
        Blood += 1;
        Toolbox.Instance.PlayerInventory.GainAmmo(AmmoType.Blood, 1);
    }

    public float Blood
    {
        get => m_blood;
        set
        {
            m_blood = value;
            Handle.ModifierPercentage = m_blood / 100f;
        }
    }
}