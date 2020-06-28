using UnityEngine;

public abstract class ProjectileInfoBase : MonoBehaviour
{
    public abstract void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, EntityType ownerType);
}