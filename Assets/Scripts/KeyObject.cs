using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour 
{
    [SerializeField] private KeyType KeyType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerInventory>().AddKey(new Key(KeyType));
            Destroy(gameObject);
        }
    }
}

public class Key : IKey
{
    public Key(KeyType keyType)
    {
        KeyType = keyType;
    }

    public KeyType KeyType { get; }
}
