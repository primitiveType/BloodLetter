using System.Collections.Generic;
using UnityEngine;

public class InstantaneousAoeDamage : MonoBehaviour, IDamageSource
{
    [SerializeField] private float DamageAmount;

    [SerializeField] private float Duration = 1f;

    [SerializeField] private float Force;

    private readonly List<Collider> hitObjects = new List<Collider>();
    [SerializeField] private float Radius;
    [SerializeField] private bool ThroughWalls = true;

    private float startTime;

    [SerializeField] private LayerMask LayersToAffect ;


    public Damage GetDamage()
    {
        return new Damage(DamageAmount, DamageType.Attack);
    }

    float IDamageSource.Force => Force;

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

        if(Physics.Raycast(position, direction, out RaycastHit info , 10,  mask))
        {
            //Debug.DrawRay(position, direction, Color.green, 5);

            
            if (!ThroughWalls && info.collider.gameObject.layer == LayerMask.NameToLayer("Default") && info.collider.gameObject != collider.gameObject)
            {
                Debug.Log($"{info.collider.name} was in the way of {collider.name} so no damage dealt");
                return;//something is obstructing the explosion
            }
            //Debug.DrawRay(position, direction, Color.magenta, 5);
        }
        

        var damaged = collider.GetComponent<IActorEvents>();

        if (collider.attachedRigidbody)
        {
            collider.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
        }

        damaged?.OnShot(this, collider.ClosestPoint(position), direction);
    }

    private void Update()
    {
        if (Time.time > startTime + Duration) enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        TryHit(other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}