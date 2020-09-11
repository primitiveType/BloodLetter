using UnityEngine;

public class OnShotParticleSpawner : MonoBehaviour
{
    [SerializeField] private ActorEvents Events;
    [SerializeField] private GameObject OnHitPrefab;

    private void Start()
    {
        Events.OnShotEvent += OnShot;
    }

    private void OnShot(object sender, OnShotEventArgs args)
    {
        if (args.ProjectileInfo.GetDamage().Type.HasFlag(DamageType.Physical))
        {
            var hitEffect = CreateHitEffect(OnHitPrefab, null);
            var adjustmentDistance = .1f;
            hitEffect.transform.position = args.WorldPos + -transform.forward * adjustmentDistance;
            hitEffect.GetComponent<Rigidbody>().AddForce(args.HitNormal * 2, ForceMode.Impulse);
        }
    }

    protected GameObject CreateHitEffect(GameObject prefab, Transform parent)
    {
        var hitEffect = Instantiate(prefab, parent, true);
        return hitEffect;
    }
}

