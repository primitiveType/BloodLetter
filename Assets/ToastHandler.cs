using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastHandler : MonoBehaviourSingleton<ToastHandler>
{
    [SerializeField] private Text toastPrefab;

    public void PopToast(string message)
    {
        var toast = Instantiate(toastPrefab, transform);
        toast.text = message;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
