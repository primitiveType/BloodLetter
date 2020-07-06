using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public bool WasFound { get; private set; }

    private void Start()
    {
        Toolbox.Instance.AddSecret(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (WasFound)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            ToastHandler.Instance.PopToast("Found Secret!");
            WasFound = true;
        }
    }
}