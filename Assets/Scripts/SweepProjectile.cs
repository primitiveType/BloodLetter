using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        gameObject.SetActive(true);
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

    protected override void PopulateData(ProjectileData data)
    {
        base.PopulateData(data);

        m_SweepMagnitude = data.SweepMagnitude;
        m_NumToSpawn = data.NumToSpawn;
        m_Duration = data.Duration;
        var attackData = GameConstants.GetProjectileDataByName(data.SubProjectileName);
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(attackData.Prefab);

        void OnHandleOnCompleted(AsyncOperationHandle<GameObject> operationHandle) => HandleOnCompleted(operationHandle);

        handle.Completed += OnHandleOnCompleted;
    }

    private void HandleOnCompleted(AsyncOperationHandle<GameObject> operationHandle)
    {
        m_ProjectileInfo = operationHandle.Result.GetComponent<ProjectileInfo>();
    }

    public override ProjectileData GetData()
    {
        var data =  base.GetData();
        var sub = m_ProjectileInfo.GetData().Name;

        data.SubProjectileName = sub;
        data.SweepMagnitude = m_SweepMagnitude;
        data.NumToSpawn = m_NumToSpawn;
        data.Duration = m_Duration;
        return data;
    }
}