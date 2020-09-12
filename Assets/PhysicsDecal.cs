using System;
using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

public class PhysicsDecal : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private GameObject particleObject;
    [SerializeField] private UltimateDecal decal;


    private void Awake()
    {
        decal.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(rb);
            GetComponent<Collider>().enabled = false;
            decal.enabled = true;
            particleObject.SetActive(false);
        }
    }
}