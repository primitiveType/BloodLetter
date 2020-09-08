using System;
using System.Collections;
using C5;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerLaserProjectile : HitscanProjectileInfo
{
    [SerializeField] private LineRenderer m_Line;
    [SerializeField] private ParticleSystem HitPoint;
    [SerializeField] private AudioSource HitAudio;

    [SerializeField] private float duration;
    [SerializeField] private float sweepMagnitude;
    [SerializeField] private bool updateTargetContinous;
    [SerializeField] private Transform ForwardTransform;

    private bool Active { get; set; }

    private void SetActive(bool active)
    {
        Active = active;
        m_Line.enabled = Active;
        if (!Active)
        {
            HitPoint.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            HitAudio.enabled = false;
        }
    }

    public void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, EntityType ownerType)
    {
        t = 0;

        UpdateAim();
        RaycastAndStuff();
        SetActive(true);
    }

    private void UpdateAim()
    {
        var slope = new Vector3(Random.Range(0, sweepMagnitude), Random.Range(0, sweepMagnitude),
            Random.Range(0, sweepMagnitude));
        var point = Owner.forward;
        var start = (point - slope).normalized;
        var end = (point + (slope / 2f)).normalized;
        SweepStart = start;
        SweepEnd = end;
    }

    public override void TriggerShoot(Transform owner, Vector3 direction, ActorRoot actorRoot)
    {
        ActorRoot = actorRoot;
        Owner = owner;
        TriggerShoot();
    }

    private void TriggerShoot()
    {
        TriggerShoot(Owner.position, Owner.forward, ActorRoot.EntityType);
    }

    private ActorRoot ActorRoot { get; set; }

    private Transform Owner { get; set; }

    private void Start()
    {
        Application.onBeforeRender += OnPreRender;
        // transform.localPosition = Vector3.zero;
        //ActorRoot = GetComponentInParent<ActorRoot>();
        //
        // TriggerShoot(transform.position, transform.forward, EntityType.Enemy);
    }

    private void OnDestroy()
    {
        Application.onBeforeRender -= OnPreRender;
    }

    private Vector3 TargetDirectionMod { get; set; }
    private Vector3 SweepStart { get; set; }
    private Vector3 SweepEnd { get; set; }
    private Vector3 hitPoint { get; set; }

    private float t;

    private void Update()
    {
        if (t < duration && Active)
        {
            if (updateTargetContinous)
            {
                UpdateAim();
            }
            var fakeT = EasingFunction.Linear(0, 1, t / duration);
            t += Time.deltaTime;

            TargetDirectionMod = Vector3.Lerp(SweepStart, SweepEnd, fakeT);
            var position = transform.position;
        }
        else
        {
            SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (t > duration)
        {
            return;
        }

        if (Active)
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
            damaged.OnShot(hit.textureCoord, hit.point,this);
            // if (OnHitPrefab)
            // {
            //     var hitEffect = GameObject.Instantiate(OnHitPrefab, damaged.transform, true);
            //     float adjustmentDistance = .1f;
            //     hitEffect.transform.position = hit.point + (hit.normal * adjustmentDistance);
            // }
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

    void OnPreRender()
    {
        //m_Line.SetPositions(new Vector3[] {transform.position, transform.position + (ForwardTransform.forward * 10)});
    }
}