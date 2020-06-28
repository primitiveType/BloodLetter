using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollide : MonoBehaviour
{
    [SerializeField] private GameObject SpawnOnCollide;
    private void OnCollisionEnter(Collision other)
    {
        Explode();
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Hit enemy!");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit Player!");
        }
        else{
            Debug.Log("Hit environment!");
        }
    }



    private void Explode()
    {
        Destroy(gameObject);
        var spawned = Instantiate(SpawnOnCollide);
        spawned.transform.position = transform.position;
    }
}
