using System.Collections.Generic;
using UnityEngine;

public abstract class InstantaneousAoe : MonoBehaviour
{
    private float startTime;
    [SerializeField] private LayerMask LayersToAffect;
    [SerializeField] private float Radius;
    [SerializeField] private bool ThroughWalls = true;
    [SerializeField] private float Duration = 1f;

    protected readonly List<Collider> hitObjects = new List<Collider>();

    private void Start()
    {
        startTime = Time.time;
        var position = transform.position;
        var overlapObjects = Physics.OverlapSphere(position, Radius, LayersToAffect);
        foreach (var collider in overlapObjects)
        {
            TryHit(collider);
        }
    }

    private void TryHit(Collider collider)
    {
        if (hitObjects.Contains(collider)) return;
        hitObjects.Add(collider);
        var position = transform.position;


        var colliderMask = LayerMaskExtensions.LayerNumbersToMask(collider.gameObject.layer);
        var mask = colliderMask.AddToMask("Default");
        var direction = collider.ClosestPoint(position) - position;

        if (Physics.Raycast(position, direction, out RaycastHit info, 10, mask))
        {
            //Debug.DrawRay(position, direction, Color.green, 5);


            if (!ThroughWalls && info.collider.gameObject.layer == LayerMask.NameToLayer("Default") &&
                info.collider.gameObject != collider.gameObject)
            {
                Debug.Log($"{info.collider.name} was in the way of {collider.name} so nothing happened");
                return; //something is obstructing the explosion
            }

            //Debug.DrawRay(position, direction, Color.magenta, 5);
            OnHit(info.collider, direction, position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        TryHit(other);
    }

    protected abstract void OnHit(Collider collider, Vector3 direction, Vector3 position);

    private void Update()
    {
        if (Time.time > startTime + Duration) enabled = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}