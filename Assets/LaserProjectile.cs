using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserProjectile : HitscanProjectileInfo
{
    [SerializeField] private LineRenderer m_Line;
    [SerializeField] private ParticleSystem HitPoint;
    [SerializeField] private AudioSource HitAudio;

    [SerializeField] private float duration;

    public void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, EntityType ownerType)
    {
        var magnitude = .25f; //adjust to change how much it sweeps

        var slope = new Vector3(Random.Range(0, magnitude), Random.Range(0, magnitude), Random.Range(0, magnitude));
        var point = ownerDirection;
        var start = (point - slope).normalized;
        var end = (point + (slope / 2f)).normalized;
        SweepStart = start;
        SweepEnd = end;
        RaycastAndStuff();
    }

    private ActorRoot ActorRoot { get; set; }

    private void Start()
    {
        transform.localPosition = Vector3.zero;
        ActorRoot = GetComponentInParent<ActorRoot>();

        TriggerShoot(transform.position, transform.forward, EntityType.Enemy);
        StartCoroutine(UpdateCr());
    }

    private Vector3 TargetDirectionMod { get; set; }
    private Vector3 SweepStart { get; set; }
    private Vector3 SweepEnd { get; set; }
    private Vector3 hitPoint { get; set; }

    private float t;

    private IEnumerator UpdateCr()
    {
        while (t < duration)
        {
            var fakeT = EasingFunction.Linear(0, 1, t / duration);
            t += Time.deltaTime;

            TargetDirectionMod = Vector3.Lerp(SweepStart, SweepEnd, fakeT);
            var position = transform.position;

            m_Line.SetPositions(new Vector3[] {position, hitPoint});
            yield return null;
        }

        HitPoint.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        HitAudio.enabled = false;

        m_Line.enabled = false;

        while (t < duration + 10)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (t > duration)
        {
            return;
        }

        RaycastAndStuff();
    }

    private void RaycastAndStuff()
    {
        var position = transform.position;
        var direction = TargetDirectionMod;
        Debug.DrawLine(position, position + direction);
        var damaged = GetHitObject(position, direction, ActorRoot, out RaycastHit hit);
        if (damaged != null)
        {
            damaged.OnShot(hit.textureCoord, this);
            if (OnHitPrefab)
            {
                var hitEffect = GameObject.Instantiate(OnHitPrefab, damaged.transform, true);
                float adjustmentDistance = .1f;
                hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
            }
        }

        bool hitSomething = hit.transform;

        bool hitEnv = hitSomething && (1 << hit.transform.gameObject.layer & EnvironmentLayers) != 0;

        m_Line.enabled = hitSomething;
        HitAudio.enabled = hitSomething;
        if (hitEnv && !HitPoint.isEmitting)
        {
            Debug.Log("Starting particles");
            //HitPoint.Play(true);
            HitPoint.Play(true);
        }

        if (!hitEnv && HitPoint.isEmitting)
        {
            Debug.Log("stopping particles");

            HitPoint.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        HitPoint.transform.position = hit.point;
        //HitPoint.transform.rotation = Quaternion.LookRotation(hit.normal);
        hitPoint = hit.point;
    }
}