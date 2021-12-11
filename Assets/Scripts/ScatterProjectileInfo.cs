using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScatterProjectileInfo : ProjectileInfoBase
{
    [SerializeField] private float degreesScatter;
    [SerializeField] private int numProjectiles;
    [SerializeField] private HitscanProjectileInfo projectileInfo;


    private void Awake()
    {
        projectileInfo = Instantiate(projectileInfo);
        projectileInfo.Force = Force;
        Debug.Log(projectileInfo.Force);
    }

    public void TriggerShoot(Vector3 playerPosition, Vector3 playerDirection, ActorRoot actorRoot, GameObject target)
    {
        //assume up vector is y axis
        for (int i = 0; i < numProjectiles; i++)
        {
            float x = Random.Range(-degreesScatter, degreesScatter);
            float y = Random.Range(-degreesScatter, degreesScatter);
            float z = Random.Range(-degreesScatter, degreesScatter);
            Vector3 position = playerDirection +
                               new Vector3(x, y, z);
            projectileInfo.TriggerShoot(playerPosition, position, actorRoot, target);
        }
    }

    public override bool TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot, GameObject target)
    {
        TriggerShoot(owner.position, direction, actorRoot, target);
        return true;
    }
}