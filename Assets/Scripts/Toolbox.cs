using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Toolbox : MonoBehaviourSingleton<Toolbox>
{
    public PlayerEvents PlayerEvents { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public Transform PlayerHeadTransform { get; private set; }

    public PlayerInventory PlayerInventory { get; private set; }
    public float TimestopTimeStamp { get; set; } = -15;
    public float TimestopDuration { get; } = 15;

    private static Toolbox s_instance;
    
    public PlayerRoot PlayerRoot { get; private set; }
    
    private void Awake()
    {
        
        DontDestroyOnLoad(this);
        LevelManager.Instance.LevelBegin += OnLevelBegin;
        LevelManager.Instance.LevelEnd += OnLevelEnd;
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
    }

    public bool IsPlayerDead { get; private set; }

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

    private List<SecretArea> Secrets = new List<SecretArea>(); //clear this out between levels
    private List<ActorHealth> Enemies = new List<ActorHealth>(); //clear this out between levels

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

public delegate void LevelEndEvent(object sender, LevelEndEventArgs args);

public class LevelEndEventArgs
{
    public LevelEndEventArgs(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}