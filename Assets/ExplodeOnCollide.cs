using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollide : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        Explode();
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Hit enemy!");
        }
        else
        {
            Debug.Log("Hit environment!");
        }
    }



    private void Explode()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position;
        Destroy(gameObject);
        
    }
}
