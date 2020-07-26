using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastHandler : MonoBehaviourSingleton<ToastHandler>
{
    [SerializeField] private Text toastPrefab;
    [SerializeField] private int max;

    public void PopToast(string message)
    {
        if (transform.childCount >= max)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        var toast = Instantiate(toastPrefab, transform);
        toast.text = message;
    }

}