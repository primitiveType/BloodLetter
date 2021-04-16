using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorTester : MonoBehaviour
{
    public Animator animator;
    public int WeaponRight;
    public int WeaponLeft;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("WeaponRight", WeaponRight);
        animator.SetInteger("WeaponLeft", WeaponLeft);
    }
}
