using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTest : MonoBehaviour
{
    private Animator _animator;

    private static readonly int Shoot = Animator.StringToHash("Shoot");

    [SerializeField] private float cooldown;

    private PlayerInventory Inventory;

    [SerializeField] private AmmoType AmmoType;
    [SerializeField] private int AmmoUsed = 1;
    
    private float shootTimestamp = -100;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        Inventory = Toolbox.PlayerInventory;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanShoot())
            {
                _animator.SetTrigger(Shoot);
                Inventory.UseAmmo(AmmoType, AmmoUsed);
            }
        }
    }

    private bool CanShoot()
    {
        return Time.time - shootTimestamp > cooldown && HasAmmo();
    }

    private bool HasAmmo()
    {
        return Inventory.GetAmmoAmount(AmmoType) > 0;
    }
}
