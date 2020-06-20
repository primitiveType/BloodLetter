using UnityEngine;

public class ScatterProjectileInfo : ProjectileInfoBase
{
    [SerializeField] private ProjectileInfo projectileInfo;
    [SerializeField] private float degreesScatter;
    [SerializeField] private int numProjectiles;

    public override void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection)
    {
        //assume up vector is y axis
        for (int i = 0; i < numProjectiles; i++)
        {
            var x = Random.Range(-degreesScatter, degreesScatter);
            var y = Random.Range(-degreesScatter, degreesScatter);
            var z = Random.Range(-degreesScatter, degreesScatter);
            var position = playerDirection +
                           new Vector3(x, y, z);
            projectileInfo.TriggerShoot(playerPosition, position);
        }
    }
}

public abstract class ProjectileInfoBase : MonoBehaviour
{
    public abstract void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection);
}