using UnityEngine;

public class OnShotParticleSpawner : MonoBehaviour
{
    [SerializeField] private ActorEvents Events;
    [SerializeField] private GameObject OnHitPrefab;

    private int numToSpawn = 2;

    private void Start()
    {
        Events.OnShotEvent += OnShot;
    }

    private void OnShot(object sender, OnShotEventArgs args)
    {
        if (args.ProjectileInfo.GetDamage().Type.HasFlag(DamageType.Physical))
        {
            for (int i = 0; i < numToSpawn; i++)
            {
                var hitEffect = CreateHitEffect(OnHitPrefab, null);
                var adjustmentDistance = .1f;
                hitEffect.transform.position = args.WorldPos + -transform.forward * adjustmentDistance;
                Vector3 forceDirection = i % 2 == 0
                    ? (args.HitNormal * Random.Range(0, 4))
                    : (args.HitNormal * Random.Range(0, -4));
                hitEffect.GetComponent<Rigidbody>()
                    .AddForce(forceDirection + (Vector3.up * Random.Range(1, 2)),
                        ForceMode.Impulse);
            }
        }
    }

    protected GameObject CreateHitEffect(GameObject prefab, Transform parent)
    {
        var hitEffect = Instantiate(prefab, parent, true);
        return hitEffect;
    }
}