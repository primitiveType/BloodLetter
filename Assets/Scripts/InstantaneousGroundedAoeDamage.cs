using UnityEngine;

public class InstantaneousGroundedAoeDamage : InstantaneousAoeDamage
{
    protected override void OnHit(Collider collider, Vector3 direction, Vector3 position)
    {
        var actorRoot = collider.GetComponentInParent<ActorRoot>();

        if (actorRoot == null || actorRoot.IsGrounded)
        {
            base.OnHit(collider, direction, position);
        }
    }
}