using System;

[Flags]
public enum DamageType
{
    Attack = 0x0001,
    Hazard = 0x0010,
    Physical = 0x0100,
    Magical = 0x1000
}