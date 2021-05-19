using System;
using UnityEngine;

public class FlinchComponent : MonoBehaviour
{
    private ActorRoot ActorRoot { get; set; }
    [SerializeField] private float DamageRequired = 1f;
    [SerializeField] private float TimeToReset = 1f;
    private static readonly int HurtBool = Animator.StringToHash("Flinching");
    [SerializeField] private Animator Animator;

    [SerializeField] private float FlinchDuration = .65f;
    private float damageTimestamp;
    private float flinchTimestamp;
    private float damageAccumulated;
    private bool _isFlinching;

    public bool IsFlinching
    {
        get => _isFlinching;
        private set
        {
            if (_isFlinching != value)
            {
                _isFlinching = value;
                Animator.SetBool(HurtBool, _isFlinching);
            }
        }
    }

    private void Awake()
    {
        EnemyDataProvider dataProvider = GetComponentInParent<EnemyDataProvider>();
        if (!dataProvider)
        {
            Debug.LogWarning($"Failed to find data provider for {name}.");
            return;
        }

        EnemyData data = dataProvider.Data;
        FlinchDuration = data.FlinchDuration;
        DamageRequired = data.DamageToFlinch;
        TimeToReset = data.TimeToResetFlinch;
    }

    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        ActorRoot.ActorEvents.OnShotEvent += OnShot;
    }

    private void Update()
    {
        if (Time.time > damageTimestamp + TimeToReset)
        {
            damageTimestamp = damageAccumulated = 0;
        }

        if (Time.time > flinchTimestamp + FlinchDuration)
        {
            IsFlinching = false;
        }
    }

    private void OnShot(object sender, OnShotEventArgs args)
    {
        damageTimestamp = Time.time;
        damageAccumulated += args.ProjectileInfo.GetDamage().Amount;

        if (damageAccumulated >= DamageRequired && !IsFlinching)
        {
            flinchTimestamp = Time.time;
            IsFlinching = true;
        }
    }
}