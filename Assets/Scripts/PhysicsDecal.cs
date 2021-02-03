﻿using System;
using System.Collections;
using System.Collections.Generic;
using Bearroll.UltimateDecals;
using UnityEngine;

public class PhysicsDecal : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private GameObject particleObject;
    [SerializeField] private GameObject floorDecalPrefab;
    [SerializeField] private GameObject wallDecalPrefab;

    [SerializeField] private LayerMask CollidesWith;

    private Vector3 previousPosition;
    private Vector3 rbPosition;

    private void Awake()
    {
        //     floorDecal.gameObject.SetActive(false);
        //     wallDecal.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        previousPosition = rbPosition;
        rbPosition = rb.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (CollidesWith.ContainsLayer(other.gameObject.layer) && !other.collider.isTrigger)
        {
            Destroy(rb);
            enabled = false;
            GetComponent<Collider>().enabled = false;
            var decalPrefab = floorDecalPrefab;

            if (IsWall(other.GetContact(0)))
            {
                decalPrefab = wallDecalPrefab;
            }

            var decal = Instantiate(decalPrefab, transform);
            particleObject.SetActive(false);
            Transform transform1 = transform;
            transform1.SetParent(other.transform);

            // var position = transform1.position;
            // transform1.up = -rb.velocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //
        // if (CollidesWith.ContainsLayer(other.gameObject.layer) && !other.isTrigger)
        // {
        //     Destroy(rb);
        //     enabled = false;
        //     GetComponent<Collider>().enabled = false;
        //     var decalPrefab = floorDecalPrefab;
        //
        //     if (IsWall(other.))
        //     {
        //         decalPrefab = wallDecalPrefab;
        //     }
        //
        //     var decal = Instantiate(decalPrefab, transform);
        //     particleObject.SetActive(false);
        //     Transform transform1 = transform;
        //     transform1.SetParent(other.transform);
        //
        //     // var position = transform1.position;
        //     // transform1.up = -rb.velocity;
        // }
    }

    private bool IsWall(ContactPoint other)
    {
        var velocityDir = (rb.position - previousPosition).normalized * .5f; // rb.velocity.normalized;
        var rayPos = previousPosition; //transform.position - (velocityDir / 2f);

        var normal = other.normal;
        var point = other.point;

        if (normal.y > .707f)
        {
            transform.up = normal;
            Debug.DrawRay(rayPos, velocityDir, Color.blue, 10);
            return false;
        }


        transform.rotation =
            Quaternion.LookRotation(Vector3.up, normal); //orientation for wall decals so blood drips down
        Debug.DrawRay(rayPos, velocityDir, Color.cyan, 10);
        return true;
        // Debug.Log($"{previousPosition}, {rb.position}");
        // if (other.Raycast(new Ray(rayPos, velocityDir),
        //     out RaycastHit info, 10f))
        // {
        //     transform.position = info.point;
        //     if (info.normal.y > .707f)
        //     {
        //         transform.up = info.normal;
        //        Debug.DrawRay(rayPos, velocityDir, Color.blue, 10);
        //         return false;
        //     }
        //
        //     transform.rotation =
        //         Quaternion.LookRotation(Vector3.up, info.normal); //orientation for wall decals so blood drips down
        //    Debug.DrawRay(rayPos, velocityDir, Color.cyan, 10);
        //     return true;
        // }
        //
        // Debug.Log("failed raycast");
        // Debug.DrawRay(rayPos, velocityDir, Color.magenta, 10);
        //
        // return false;
    }
}