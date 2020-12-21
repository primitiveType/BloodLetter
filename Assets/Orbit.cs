using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Orbit : MonoBehaviour
{
    public Transform m_RotateAround;

    public float m_Speed;

    public Vector3 m_Axis;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(m_RotateAround.position, m_Axis, m_Speed);
    }
}
