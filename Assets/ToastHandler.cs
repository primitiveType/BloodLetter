using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastHandler : MonoBehaviour
{
    [SerializeField] private Text toastPrefab;

    public static ToastHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

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
