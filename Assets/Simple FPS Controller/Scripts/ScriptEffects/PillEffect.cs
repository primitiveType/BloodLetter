using System;

[Serializable]
public class PillEffect
{
    public float chanceToHealth = 50.0f; // The chance for that drug to heal on taking
    public float contrast; // Visual effect
    public float duration = 20.0f; // In seconds
    public float focusDistance = 3.5f;
    public float health = 30.0f; // The health that this drug heals

    public float hueShift = 10.0f; // Visual effect
    public float saturation; // Visual effect
}