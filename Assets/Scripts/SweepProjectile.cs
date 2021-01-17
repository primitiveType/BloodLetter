using System.Collections;
using UnityEngine;

public class SweepProjectile : ProjectileInfo
{
    [SerializeField] private ProjectileInfo m_ProjectileInfo;
    [SerializeField] private float m_Duration;
    [SerializeField] private float m_SweepMagnitude = .25f;
    [SerializeField] private int m_NumToSpawn;

    private ActorRoot ActorRoot { get; set; }

    private Vector3 TargetDirectionMod { get; set; }
    private Vector3 SweepStart { get; set; }
    private Vector3 SweepEnd { get; set; }

    private Coroutine ShootCoroutine { get; set; }
    
    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        var slope = new Vector3(Random.Range(0, m_SweepMagnitude), Random.Range(0, 0), Random.Range(0, m_SweepMagnitude));
        var point = direction;
        var start = (point - slope).normalized;
        var end = (point + slope / 2f).normalized;
        SweepStart = start;
        SweepEnd = end;
        if (ShootCoroutine != null)
        {
            StopCoroutine(ShootCoroutine);
        }
        ShootCoroutine = StartCoroutine(UpdateCr());
    }

    private void Start()
    {
        transform.localPosition = Vector3.zero;
        ActorRoot = GetComponentInParent<ActorRoot>();

      //  TriggerShoot(transform, transform.forward, EntityType.Enemy);
    }

    private IEnumerator UpdateCr()
    {
        float t = 0;
        float timeToWait = m_Duration / m_NumToSpawn;
        while (t < m_Duration)
        {
            var fakeT = EasingFunction.Linear(0, 1, t / m_Duration);
            t += Time.deltaTime;

            TargetDirectionMod = Vector3.Lerp(SweepStart, SweepEnd, fakeT);
            yield return new WaitForSeconds(timeToWait);
            FireProjectile();
        }

 

        while (t < m_Duration + 10) yield return null;

    }



    private void FireProjectile()
    {
        var position = transform.position;
        var direction = TargetDirectionMod;
        Debug.DrawLine(position, position + direction);
        m_ProjectileInfo.TriggerShoot(transform, direction, ActorRoot);
    }
}