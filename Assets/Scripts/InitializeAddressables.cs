using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitializeAddressables : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Addressables.InitializeAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
