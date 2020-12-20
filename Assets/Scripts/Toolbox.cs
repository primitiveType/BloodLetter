using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Toolbox : MonoBehaviourSingleton<Toolbox>
{
    private static Toolbox s_instance;
    private readonly List<ActorHealth> Enemies = new List<ActorHealth>(); //clear this out between levels

    private readonly List<SecretArea> Secrets = new List<SecretArea>(); //clear this out between levels
    public PlayerEvents PlayerEvents { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Transform PlayerHeadTransform { get; private set; }

    public PlayerInventory PlayerInventory { get; private set; }
    public float TimestopTimeStamp { get; set; } = -15;
    public float TimestopDuration { get; } = 15;

    public PlayerRoot PlayerRoot { get; private set; }
    

    public bool IsPlayerDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LevelManager.Instance.LevelBegin += OnLevelBegin;
        LevelManager.Instance.LevelEnd += OnLevelEnd;
        TaskScheduler.UnobservedTaskException -= TaskSchedulerOnUnobservedTaskException;
        TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
    }

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Debug.LogError($"Caught exception in unobserved task : {e.Exception.InnerException.Message}");
    }


    private void OnLevelEnd(object sender, LevelEndEventArgs args)
    {
        if (args.Success)
        {
        }
    }

    private void OnLevelBegin(object sender, LevelBeginEventArgs args)
    {
        IsPlayerDead = false;
        Secrets.Clear();
        Enemies.Clear();
    }

    public bool TimeIsStopped()
    {
        return TimestopTimeStamp + TimestopDuration > Time.unscaledTime;
    }


    public void SetPlayerEvents(PlayerEvents events)
    {
        PlayerEvents = events;
        PlayerEvents.OnDeathEvent -= PlayerEventsOnOnDeathEvent;
        PlayerEvents.OnDeathEvent += PlayerEventsOnOnDeathEvent;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.LevelBegin -= OnLevelBegin;
        LevelManager.Instance.LevelEnd -= OnLevelEnd;
        TaskScheduler.UnobservedTaskException -= TaskSchedulerOnUnobservedTaskException;
    }

    private void PlayerEventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        IsPlayerDead = true;
        StartCoroutine(EndLevelAfterTwoSeconds());
    }

    private IEnumerator EndLevelAfterTwoSeconds()
    {
        var ppHandle = PostProcessingManager.Instance.EnableDeathEffect();
        yield return new WaitForSeconds(2);
        ppHandle.Dispose();
        LevelManager.Instance.EndLevel(false);
    }

    public void SetPlayerTransform(Transform transform)
    {
        PlayerTransform = transform;
    }

    public void SetPlayerHeadTransform(Transform transform)
    {
        PlayerHeadTransform = transform;
    }

    public void SetPlayerInventory(PlayerInventory inventory)
    {
        PlayerInventory = inventory;
    }

    public void AddSecret(SecretArea secret)
    {
        Secrets.Add(secret);
    }

    public void GetSecretStatus(out int totalSecrets, out int foundSecrets)
    {
        totalSecrets = Secrets.Count;
        foundSecrets = Secrets.Count(s => s.WasFound);
    }

    public void AddEnemy(ActorHealth enemy)
    {
        Enemies.Add(enemy);
    }

    public void GetEnemyStatus(out int totalEnemies, out int deadEnemies)
    {
        totalEnemies = Enemies.Count;
        deadEnemies = Enemies.Count(enemy => !enemy.IsAlive);
    }


    private void Update()
    {
        //Time.timeScale = TimeIsStopped() ? 0f : 1f;
    }

    public void SetPlayerRoot(PlayerRoot playerRoot)
    {
        PlayerRoot = playerRoot;
    }
}