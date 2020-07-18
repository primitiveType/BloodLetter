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

    private EquipStatus CurrentEquip; //will have to make changes if you can later equip multiple things
    private static Toolbox s_instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public bool TimeIsStopped()
    {
        return TimestopTimeStamp + TimestopDuration > Time.unscaledTime;
    }

    public void EquipThing(EquipStatus thing)
    {
        Instance.StartCoroutine(EquipThingCR(thing));
    }

    private IEnumerator EquipThingCR(EquipStatus thing)
    {
        if (CurrentEquip == thing || !thing.CanEquip())
        {
            yield break;
        }

        if (CurrentEquip != null)
        {
            yield return StartCoroutine(CurrentEquip.UnEquip());
        }

        yield return StartCoroutine(thing.Equip());
        CurrentEquip = thing;
    }

    public void SetPlayerEvents(PlayerEvents events)
    {
        PlayerEvents = events;
        PlayerEvents.OnDeathEvent -= PlayerEventsOnOnDeathEvent;
        PlayerEvents.OnDeathEvent += PlayerEventsOnOnDeathEvent;
    }

    public bool IsPlayerDead { get; private set; }

    private void PlayerEventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        IsPlayerDead = true;
        StartCoroutine(EndLevelAfterTwoSeconds());
    }

    private IEnumerator EndLevelAfterTwoSeconds()
    {
        yield return new WaitForSeconds(2);
        LevelManager.Instance.EndLevel();
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

    public void CleanupForNextLevel()
    {
        IsPlayerDead = false;
        Secrets.Clear();
        Enemies.Clear();
    }

    private void Update()
    {
        Time.timeScale = TimeIsStopped() ? 0f : 1f;
    }
}