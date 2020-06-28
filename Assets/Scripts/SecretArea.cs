using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Found Secret!");
            //trigger secret area text, etc.
        }        
    }
}
