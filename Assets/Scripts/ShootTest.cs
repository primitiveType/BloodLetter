using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootTest : MonoBehaviour
{
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Shooting = Animator.StringToHash("Shooting");
    [SerializeField] private Animator m_Animator;

    private Animator Animator => m_Animator;
    [SerializeField] private float cooldown;

    private EquipStatus Status { get; set; }

    private float ShootTimestamp { get; set; } = -100;

    private List<UsesAmmo> UsesAmmo { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        //_animator = GetComponentInChildren<Animator>();
        UsesAmmo = GetComponents<UsesAmmo>().ToList();
        Status = GetComponent<EquipStatus>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale <= 0 || !Status.IsEquipped) return;
        if (!Input.GetMouseButton(0) || !CanShoot())
        {
            Animator.SetBool(Shooting, false);
            return;
        }

        ShootTimestamp = Time.time;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("shoot trigger");


            Animator.SetTrigger(Shoot);
        }

        Animator.SetBool(Shooting, true);
        // Inventory.UseAmmo(AmmoType, AmmoUsed);//this should not be here
    }

    private bool CanShoot()
    {
        return Time.time - ShootTimestamp > cooldown && HasAmmo();
    }

    private bool HasAmmo()
    {
        var hasAmmo = true;
        foreach (var usesAmmo in UsesAmmo) hasAmmo &= usesAmmo.HasAmmo();

        return hasAmmo;
    }
}