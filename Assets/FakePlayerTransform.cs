using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Toolbox.Instance.SetPlayerTransform(transform);
        Toolbox.Instance.SetPlayerHeadTransform(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
