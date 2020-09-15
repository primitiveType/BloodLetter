using System.Collections.Generic;
using UnityEngine;

public class InstantaneousAoeDamage : MonoBehaviour, IDamageSource
{
    [SerializeField] private float DamageAmount;

    [SerializeField] private float Duration = 1f;

    [SerializeField] private float Force;

    private readonly List<Collider> hitObjects = new List<Collider>();
    [SerializeField] private float Radius;

    private float startTime;


    public Damage GetDamage()
    {
        return new Damage(DamageAmount, DamageType.Attack);
    }

    float IDamageSource.Force => Force;

    private void Start()
    {
        startTime = Time.time;
        var position = transform.position;
        var overlapObjects = Physics.OverlapSphere(position, Radius, ~LayerMask.NameToLayer("Default"));
        foreach (var collider in overlapObjects)
        {
            if (hitObjects.Contains(collider)) continue;

            hitObjects.Add(collider);

            Debug.Log($"hit {collider.name}");
            var damaged = collider.GetComponent<IActorEvents>();
            var direction = collider.transform.position - transform.position;

            if (collider.attachedRigidbody)
            {
                collider.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
            }

            damaged?.OnShot(this, collider.ClosestPoint(position), direction);
        }
    }

    private void Update()
    {
        if (Time.time > startTime + Duration) enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled || hitObjects.Contains(other)) return;

        hitObjects.Add(other);

        Debug.Log($"hit {other.name}");
        var damaged = other.GetComponentInChildren<IActorEvents>();
        var position = transform.position;
        var direction = other.transform.position - position;

        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
        }

        damaged?.OnShot(this, other.ClosestPoint(position), direction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}