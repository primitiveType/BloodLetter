using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class OnShotParticleSpawner : MonoBehaviour
{
    [SerializeField] private ActorEvents Events;
    [SerializeField] private GameObject OnHitPrefab;

    private int numToSpawn = 2;

    private void Start()
    {
        Events.OnShotEvent += OnShot;
    }

    private float Variance = .5f;

    private void OnShot(object sender, OnShotEventArgs args)
    {
        if (args.ProjectileInfo.GetDamage().Type.HasFlag(DamageType.Physical))
        {
            var force = Mathf.Max(args.ProjectileInfo.Force, 1f);//always apply at least 1 to blood
            force *= .1f;//magic multiplier
            for (int i = 0; i < numToSpawn; i++)
            {
                var hitEffect = CreateHitEffect(OnHitPrefab, null);
                var adjustmentDistance = .01f;
                hitEffect.transform.position = args.WorldPos + -transform.forward * adjustmentDistance;
                Vector3 adjustedNormal = args.HitNormal + new Vector3(Random.Range(-Variance, Variance),
                    Random.Range(-Variance, Variance), Random.Range(-Variance, Variance));
                Vector3 forceDirection = i % 2 == 0
                    ? (adjustedNormal * Random.Range(0, force))
                    : (adjustedNormal * Random.Range(0, -force));
                hitEffect.GetComponent<Rigidbody>()
                    .AddForce(forceDirection + (Vector3.up * Random.Range(1, force/2f)),
                        ForceMode.Impulse);
            }
        }
    }

    protected GameObject CreateHitEffect(GameObject prefab, Transform parent)
    {
        var hitEffect = Instantiate(prefab, parent, true);
        return hitEffect;
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= OnShot;

    }
}