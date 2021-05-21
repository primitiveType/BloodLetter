using System;

[Flags]
public enum WeaponId
{
    Pistol = 1,
    Shotgun = 1<<1,
    Staff = 1<<2,
    Chainsaw = 1<<3,
    Eye = 1<<4,
    Lightning = 1<<5,
    ShotgunStaff = 1<<6,
    Syringe  = 1<<7
}