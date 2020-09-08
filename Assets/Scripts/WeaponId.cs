using System;

[Flags]
public enum WeaponId
{
    Pistol = 0x0001,
    Shotgun = 0x0010,
    Staff = 0x0100,
    Chainsaw = 0x0001000,
    Eye = 0x0010000,
    Lightning = 0x0100000,
    ShotgunStaff = 0x1000000
}