﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private List<AudioClip> aggroClips;

    [FormerlySerializedAs("stepClip")] [SerializeField]
    private List<AudioClip> stepClips;

    [FormerlySerializedAs("hurtClip")] [SerializeField]
    private List<AudioClip> hurtClips;

    [FormerlySerializedAs("attackClip")] [SerializeField]
    private List<AudioClip> attackClips;

    [FormerlySerializedAs("attackClip")] [SerializeField]
    private List<AudioClip> deathClips;

    public AudioClip StepClip => stepClips.Random();
    public AudioClip HurtClip => hurtClips.Random();
    public AudioClip AttackClip => attackClips.Random();
    public AudioClip DeathClip => deathClips.Random();
    public AudioClip AggroClip => aggroClips.Random();

    private AudioSource StepSource { get; set; }
    private AudioSource HurtSource { get; set; }
    private AudioSource AttackSource { get; set; } //I don't think its smart to have this many sources.

    private ActorEvents Events;

    private void Start()
    {
        Events = GetComponent<ActorEvents>();
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnShotEvent += OnEnemyShot;
        Events.OnAttackEvent += OnEnemyAttack;
        Events.OnDeathEvent += OnEnemyDeath;
        Events.OnAggroEvent += OnEnemyAggro;
        StepSource = CreateAudioSource();
        HurtSource = CreateAudioSource();
        AttackSource = CreateAudioSource();
    }

    private AudioSource CreateAudioSource()
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.spatialize = true;
        source.spatialBlend = 1;
        return source;
    }

    private void OnEnemyAggro(object sender, OnAggroEventArgs args)
    {
        if (!HurtSource.isPlaying)
        {
            HurtSource.PlayOneShot(AggroClip);
        }
    }

    private void OnEnemyDeath(object sender, OnDeathEventArgs args)
    {
        DetachEvents();
        HurtSource.PlayOneShot(DeathClip);
        // HurtSource.Stop();
        // HurtSource.clip = DeathClip;
        // HurtSource.Play();
    }

    private void OnEnemyAttack(object sender, OnAttackEventArgs args)
    {
        AttackSource.PlayOneShot(AttackClip); //todo: only play if player hit by it?
    }

    private void OnEnemyShot(object sender, OnShotEventArgs args)
    {
        HurtSource.clip = HurtClip;
        HurtSource.Play();
    }

    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
        StepSource.PlayOneShot(StepClip);
    }

    private void OnDestroy()
    {
        DetachEvents();
    }

    private void DetachEvents()
    {
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnShotEvent -= OnEnemyShot;
        Events.OnAttackEvent -= OnEnemyAttack;
        Events.OnDeathEvent -= OnEnemyDeath;
        Events.OnAggroEvent -= OnEnemyAggro;
    }
}

public static class ListExtensions
{
    public static T Random<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            return default(T);
        }

        int index = UnityEngine.Random.Range(0, list.Count());
        return list[index];
    }
}