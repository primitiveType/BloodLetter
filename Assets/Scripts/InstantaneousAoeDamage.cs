using UnityEngine;

public class InstantaneousAoeDamage : InstantaneousAoe, IDamageSource
{
    [SerializeField] private float DamageAmount;


    [SerializeField] private float Force;


    public Damage GetDamage()
    {
        return new Damage(DamageAmount, DamageType.Attack);
    }

    float IDamageSource.Force => Force;


    protected override void OnHit(Collider collider, Vector3 direction, Vector3 position)
    {
        var damaged = collider.GetComponentInChildren<IActorEvents>();

        if (collider.attachedRigidbody)
        {
            collider.attachedRigidbody.AddForce(direction * Force, ForceMode.Impulse);
        }

        damaged?.OnShot(this, collider.ClosestPoint(position), direction);
    }
}