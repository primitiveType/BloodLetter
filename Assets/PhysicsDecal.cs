using System;
using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

public class PhysicsDecal : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private GameObject particleObject;
    [SerializeField] private UltimateDecal floorDecal;
    [SerializeField] private UltimateDecal wallDecal;

    [SerializeField] private LayerMask CollidesWith;

    private void Awake()
    {
        floorDecal.gameObject.SetActive(false);
        wallDecal.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CollidesWith.ContainsLayer(other.gameObject.layer))
        {
            Destroy(rb);
            GetComponent<Collider>().enabled = false;
            var decal = floorDecal;

            if (IsWall(other))
            {
                decal = wallDecal;
            }

            decal.gameObject.SetActive(true);
            particleObject.SetActive(false);
            Transform transform1 = transform;
            transform1.SetParent(other.transform);

            // var position = transform1.position;
            // transform1.up = -rb.velocity;
        }
    }

    private bool IsWall(Collider other)
    {
        var position = transform.position;

        var otherPoint = other.ClosestPointOnBounds(position);
        Debug.Log(otherPoint - position);
        if (other.Raycast(new Ray(position, (otherPoint - position).normalized),
            out RaycastHit info, 10f))
        {
            if (info.normal.y > .707f)
            {
                transform.up = info.normal;
                Debug.DrawRay(position, otherPoint - position, Color.blue, 10);
                return false;
            }
            transform.rotation = Quaternion.LookRotation( Vector3.up, info.normal);//orientation for wall decals so blood drips down
            Debug.DrawRay(position, otherPoint - position, Color.cyan, 10);
            return true;
        }

        Debug.Log("failed raycast");
        Debug.DrawRay(position, otherPoint - position, Color.magenta, 10);

        return false;
    }
}