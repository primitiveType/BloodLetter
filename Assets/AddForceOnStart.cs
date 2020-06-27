using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnStart : MonoBehaviour
{
    [SerializeField] private float ForceToAdd;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(ForceToAdd * transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
