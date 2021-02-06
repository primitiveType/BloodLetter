using UnityEngine;

public interface IDamageSource
{
    Damage GetDamage();
    float Force { get; }
    
}