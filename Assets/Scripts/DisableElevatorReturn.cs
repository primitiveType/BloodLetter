using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableElevatorReturn : MonoBehaviour
{
    [SerializeField] private Elevator Elevator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            Elevator.returns = false;
    }
}