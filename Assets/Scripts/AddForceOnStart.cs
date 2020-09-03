using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnStart : MonoBehaviour
{
    [SerializeField] private float ForceToAdd;

    [SerializeField] private float lobBias;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 dir = transform.forward;
        Vector3 lobAmount = transform.up * lobBias;
        dir = (dir + lobAmount).normalized;
        GetComponent<Rigidbody>().AddForce(ForceToAdd * dir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
