using System;
using System.Collections;
using SensorToolkit;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAggroHandler : MonoBehaviour
{
    private static readonly int Aggro = Animator.StringToHash("IsAggro");

    [SerializeField] public float AggroDelayVariance = 1f;

    [SerializeField] public float AggroRange;
    [SerializeField] public float EarshotAggroRange = 20;


    private bool isInitialized;
    private float m_distance;
    private bool m_isAggro;
    [SerializeField] private IMonsterVisibilityHandler VisibilityHandler;
    public IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot { get; set; }
    private Animator Animator => ActorRoot.Animator;

    public bool IsAggro
    {
        get => m_isAggro;
        set
        {
            Initialize();
            var prevAggro = IsAggro;
            m_isAggro = value;
            Animator.SetBool(Aggro, IsAggro);
            if (IsAggro && !prevAggro) //this event should take args or there should be a deaggro event
                Events.OnAggro();

            PreAggro = value; //if un-aggro'd, reset this
        }
    }

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            Debug.LogWarning($"Failed to find data provider for {name}.");
        }

        if (!enabled)
        {
            OnDisable();
        }
        EnemyData data = dataProvider.Data;
        enabled = enabled && data.CanAggro;
        AggroRange = data.AggroRange;
        AggroDelayVariance = data.AggroDelayVariance;
        EarshotAggroRange = data.EarshotAggroRange;
        VisibilityHandler = GetComponent<IMonsterVisibilityHandler>();
    }

    private bool PreAggro { get; set; }


    public Transform Target { get; set; }

    private void OnPlayerShoot(object sender, PlayerShootEventArgs args)
    {
        if (Vector3.Distance(Target.position, transform.position) > EarshotAggroRange) return;

        if (VisibilityHandler.CanSeePlayer(true, true)) SetAggro();
    }

    private void OnDisable()
    {
        IsAggro = false;
    }

    public void OnDetected(GameObject go, Sensor sensor)
    {
        if (ShouldAggroTo(go))
        {
            SetAggro();
        }
    }
    
    public void OnLostDetection(GameObject go, Sensor sensor)
    {
       
    }

    private bool ShouldAggroTo(GameObject go)
    {
        return this.enabled && go.transform == Toolbox.Instance.PlayerTransform;
    }

    public void OnEnable()
    {
        Debug.Log("enabled.");
    }
    
    public void SetAggro()
    {
        if (!PreAggro)
        {
            PreAggro = true;
            StartCoroutine(AggroAfterRandomDelay());
        }
    }

    public void LoseAggro()
    {
        
    }

    private IEnumerator AggroAfterRandomDelay()
    {
        yield return new WaitForSeconds(Random.Range(0, AggroDelayVariance));
        IsAggro = true;
    }

    private void Initialize()
    {
        if (isInitialized) return;

        isInitialized = true;

        ActorRoot = GetComponentInParent<ActorRoot>();
        Toolbox.Instance.PlayerEvents.PlayerShootEvent += OnPlayerShoot;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnDeathEvent += OnEnemyDeath;

        Target = Toolbox.Instance.PlayerTransform;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        m_distance = Vector3.Distance(Target.transform.position, transform.position);
        //if (m_distance < AggroRange && VisibilityHandler.CanSeePlayer()) SetAggro();
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        IsAggro = false;
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        IsAggro = true; //do we need to check if alive?
        VisibilityHandler.LastSeenPosition =
            Toolbox.Instance.PlayerHeadTransform.position; //hack, should be based on who shot them. 
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnDeathEvent -= OnEnemyDeath;
    }
}