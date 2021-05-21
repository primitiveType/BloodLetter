using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Shooting = Animator.StringToHash("Shooting");

    private static readonly int Cast = Animator.StringToHash("Cast");
    private static readonly int Casting = Animator.StringToHash("Casting");
    [SerializeField] private Animator m_Animator;

    private Animator Animator => m_Animator;
    [SerializeField] private float cooldown;

    private EquipStatus Status { get; set; }

    private float ShootTimestamp { get; set; } = -100;

    private List<IShootCondition> UsesAmmo { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        //_animator = GetComponentInChildren<Animator>();
        UsesAmmo = GetComponents<IShootCondition>().ToList();
        Status = GetComponent<EquipStatus>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale <= 0 || !Status.IsEquipped) return;

        if (Status.WeaponId.HasFlag(WeaponId.Shotgun))
        {
            HandleShoot();
        }
        else
        {
            HandleCast();
        }

        // Inventory.UseAmmo(AmmoType, AmmoUsed);//this should not be here
    }

    private void HandleCast()
    {
        if (!Input.GetMouseButton(1) || !CanShoot())
        {
            Animator.SetBool(Casting, false);
            return;
        }

        ShootTimestamp = Time.time;
        if (Input.GetMouseButtonDown(1))
        {
            // Debug.Log("shoot trigger");
                Animator.SetTrigger(Cast);
        }

        Animator.SetBool(Casting, true);
    }

    private void HandleShoot()
    {
        if (!Input.GetMouseButton(0) || !CanShoot())
        {
            Animator.SetBool(Shooting, false);
            return;
        }

        ShootTimestamp = Time.time;
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("shoot trigger");
            Animator.SetTrigger(Shoot);
        }

        Animator.SetBool(Shooting, true);
    }


    private bool CanShoot()
    {
        return Time.time - ShootTimestamp > cooldown && HasAmmo();
    }

    private bool HasAmmo()
    {
        var hasAmmo = true;
        foreach (var usesAmmo in UsesAmmo) hasAmmo &= usesAmmo.CanShoot();

        return hasAmmo;
    }
}