using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceOverTime : MonoBehaviour
{
    public Rigidbody rb;

    public Vector3 force;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb == null)
        {
            Destroy(this);
            return;
        }
        rb.AddForce(force);
    }
}
