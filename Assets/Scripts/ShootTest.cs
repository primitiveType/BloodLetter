using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Shooting = Animator.StringToHash("Shooting");
    [SerializeField] private Animator _animator;

    [SerializeField] private float cooldown;

    private float shootTimestamp = -100;

    private List<UsesAmmo> UsesAmmo;

    // Start is called before the first frame update
    private void Start()
    {
        //_animator = GetComponentInChildren<Animator>();
        UsesAmmo = GetComponents<UsesAmmo>().ToList();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale <= 0) return;
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
        var hasAmmo = true;
        foreach (var usesAmmo in UsesAmmo) hasAmmo &= usesAmmo.HasAmmo();

        return hasAmmo;
    }
}