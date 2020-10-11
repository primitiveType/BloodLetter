using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private List<AudioClip> aggroClips;

    [FormerlySerializedAs("attackClip")] [SerializeField]
    private List<AudioClip> attackClips;

    [FormerlySerializedAs("attackClip")] [SerializeField]
    private List<AudioClip> deathClips;

    private IActorEvents Events;

    [FormerlySerializedAs("hurtClip")] [SerializeField]
    private List<AudioClip> hurtClips;

    [SerializeField] private AudioSource m_attackSource;
    [SerializeField] private AudioSource m_hurtSource;
    [SerializeField] private AudioSource m_stepSource;

    [FormerlySerializedAs("stepClip")] [SerializeField]
    private List<AudioClip> stepClips;

    public AudioClip StepClip => stepClips.Random();
    public AudioClip HurtClip => hurtClips.Random();
    public AudioClip AttackClip => attackClips.Random();
    public AudioClip DeathClip => deathClips.Random();
    public AudioClip AggroClip => aggroClips.Random();

    private AudioSource StepSource
    {
        get => m_stepSource;
        set => m_stepSource = value;
    }

    private AudioSource HurtSource
    {
        get => m_hurtSource;
        set => m_hurtSource = value;
    }

    private AudioSource AttackSource
    {
        get => m_attackSource;
        set => m_attackSource = value;
    } //I don't think its smart to have this many sources.

    private void Start()
    {
        Events = GetComponent<IActorEvents>();
        Events.OnStepEvent += OnEnemyStepped;
        Events.OnHealthChangedEvent += OnEnemyDamaged;
        Events.OnAttackEvent += OnEnemyAttack;
        Events.OnDeathEvent += OnEnemyDeath;
        Events.OnAggroEvent += OnEnemyAggro;


        if (!StepSource) StepSource = CreateAudioSource();

        if (!HurtSource) HurtSource = CreateAudioSource();

        if (!AttackSource) AttackSource = CreateAudioSource();
    }

    private IEnumerable GetSources()
    {
        yield return StepSource;
        yield return HurtSource;
        yield return AttackSource;
    }

    private AudioSource CreateAudioSource()
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.spatialize = true;
        source.spatialBlend = .75f;
        return source;
    }

    private void OnEnemyAggro(object sender, OnAggroEventArgs args)
    {
        if (!HurtSource.isPlaying) HurtSource.PlayOneShot(AggroClip);
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

    private void OnEnemyDamaged(object sender, OnHealthChangedEventArgs args)
    {
        if (args.IsHealing)
            return;
        HurtSource.clip = HurtClip;
        HurtSource.Play();
    }

    private void OnEnemyStepped(object sender, OnStepEventArgs args)
    {
        var clip = StepClip;
        if (clip)
            StepSource.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        DetachEvents();
    }

    private void DetachEvents()
    {
        Events.OnStepEvent -= OnEnemyStepped;
        Events.OnHealthChangedEvent -= OnEnemyDamaged;
        Events.OnAttackEvent -= OnEnemyAttack;
        Events.OnDeathEvent -= OnEnemyDeath;
        Events.OnAggroEvent -= OnEnemyAggro;
    }
}