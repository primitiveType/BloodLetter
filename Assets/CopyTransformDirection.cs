using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransformDirection : MonoBehaviour
{
    [SerializeField] private Transform toCopy;

    private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.rotation = toCopy.rotation;
    }
}
