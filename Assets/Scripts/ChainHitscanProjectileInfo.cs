using UnityEngine;

public class ChainHitscanProjectileInfo : HitscanProjectileInfo
{
    [SerializeField] public int NumHits = 2;
    [SerializeField] public float Radius = 10;

    protected override GameObject CreateHitEffect(GameObject prefab, Transform parent, RaycastHit hit)
    {
        var effect = base.CreateHitEffect(prefab, parent, hit);
        NumHits--;
        if (NumHits <= 0) return effect;

        var chain = effect.GetComponent<ChainHitscanProjectileInfo>();
        if (chain)
        {
            chain.NumHits = NumHits;
            Collider[] hits = Physics.OverlapSphere(hit.point, Radius, LayerMask.GetMask("Enemy", "EnemyRaycast"),
                QueryTriggerInteraction.Collide);
            if (hits.Length > 0)
            {
                chain.TriggerShoot(hit.point, hits[0].transform.position - hit.point, ownerRoot);
            }
        }

        return effect;
    }
}