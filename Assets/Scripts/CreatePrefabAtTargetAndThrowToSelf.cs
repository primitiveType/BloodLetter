using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePrefabAtTargetAndThrowToSelf : MonoBehaviour, IInteractable
{
    public GameObject Prefab;
    public GameObject Target;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, .1f);
    }

    public bool Interact()
    {
        var thing = Instantiate(Prefab);
        thing.transform.position = Target.transform.position;
        var rb = thing.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce((this.transform.position - thing.transform.position) * Speed);
        }

        return true;
    }
}