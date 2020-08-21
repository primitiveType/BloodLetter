using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    [SerializeField]private Animator _animator;

    private static readonly int Shoot = Animator.StringToHash("Shoot");

    [SerializeField] private float cooldown;

    private float shootTimestamp = -100;
    private static readonly int Shooting = Animator.StringToHash("Shooting");

    private List<UsesAmmo> UsesAmmo;

    // Start is called before the first frame update
    void Start()
    {
        //_animator = GetComponentInChildren<Animator>();
        UsesAmmo = GetComponents<UsesAmmo>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale <= 0)
        {
            return;
        }
        if (!Input.GetMouseButton(0) || !CanShoot())
        {
            _animator.SetBool(Shooting, false);
            return;
        }

        shootTimestamp = Time.time;
        _animator.SetBool(Shooting, true);
        // Inventory.UseAmmo(AmmoType, AmmoUsed);//this should not be here
    }

    private bool CanShoot()
    {
        return Time.time - shootTimestamp > cooldown && HasAmmo();
    }

    private bool HasAmmo()
    {
        bool hasAmmo = true;
        foreach (var usesAmmo in UsesAmmo)
        {
            hasAmmo &= usesAmmo.HasAmmo();
        }
        
        return hasAmmo;
    }
}