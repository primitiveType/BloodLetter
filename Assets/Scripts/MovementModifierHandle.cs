public class MovementModifierHandle
{
    private float m_modifierPercentage;

    public float ModifierPercentage
    {
        get => m_modifierPercentage;
        set
        {
            m_modifierPercentage = value;
            OnHandleChanged?.Invoke(this, new HandleChangedArgs());
        }
    }

    public event HandleChanged OnHandleChanged;
}