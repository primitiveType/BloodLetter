using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserProjectile : HitscanProjectileInfo
{
    [SerializeField] private LineRenderer m_Line;
    [SerializeField] private GameObject HitPoint;
    
    [SerializeField] private float duration;

    public void TriggerShoot(Vector3 ownerPosition, Vector3 ownerDirection, EntityType ownerType)
    {
        
        var magnitude = .25f;//adjust to change how much it sweeps

        var slope = new Vector3(Random.Range(0, magnitude), Random.Range(0, magnitude ), Random.Range(0, magnitude));
        var point = ownerDirection;
        var start = (point - slope).normalized;
        var end = (point + (slope/2f)).normalized;
        SweepStart = start ;
        SweepEnd = end ;
        RaycastAndStuff();
    }

    private ActorRoot ActorRoot { get; set; }
    private void Start()
    {
        transform.localPosition = Vector3.zero;
        ActorRoot = GetComponentInParent<ActorRoot>();
        
        TriggerShoot(transform.position, transform.forward, EntityType.Enemy);
    }

    private Vector3 TargetDirectionMod { get; set; }
    private Vector3 SweepStart { get; set; }
    private Vector3 SweepEnd { get; set; }
    private Vector3 hitPoint { get; set; }

    private float t;
    private void Update()
    {
        var fakeT = EasingFunction.Linear(0, 1, t / duration);
        t += Time.deltaTime;

        TargetDirectionMod = Vector3.Lerp(SweepStart, SweepEnd, fakeT);
        if (t > duration)
        {
            Destroy(gameObject);
        }
        var position = transform.position;

        m_Line.SetPositions(new Vector3[]{position, hitPoint});
        if (m_Line.GetPosition(0) != transform.position)
        {
            Debug.LogWarning("Position was wrong!");
        }
    }

    private void FixedUpdate()
    {
        RaycastAndStuff();
    }

    private void RaycastAndStuff()
    {
        var position = transform.position;
        var direction = TargetDirectionMod;
        Debug.DrawLine(position, position + direction);
        var damaged = GetHitObject(position, direction, ActorRoot , out RaycastHit hit);
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

        bool gotHit = hit.transform;

        m_Line.enabled = gotHit;
        HitPoint.SetActive(gotHit);
        HitPoint.transform.position = hit.point;
        Debug.Log(position);
        hitPoint = hit.point;
    }
}