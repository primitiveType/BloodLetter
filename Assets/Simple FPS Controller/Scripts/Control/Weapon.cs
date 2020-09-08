using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Weapon
{
    public uint ammo = 600;

    [Header("Effects")] public Text AmmoUI;

    public uint leftAmmoInFiller = 30;

    [Header("Specification")] public uint maxAmmoInAFiller = 30;

    public ParticleSystem muzzleFlash;
    public float reloadTime = 1;
    public float shootTime;
    public ArmedWeapon weapon;
    public Transform WeaponFiller;

    [Header("Objects")] public GameObject WeaponObj;
}