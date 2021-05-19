using UnityEngine;

public class ActorRoot : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public Animator Animator
    {
        get => _animator;
        private set => _animator = value;
    }

    public IActorEvents ActorEvents { get; private set; }

    public AnimationMaterialHelper AnimationMaterialHelper { get; private set; }
    public IDamagedByHitscanProjectile[] HitscanColliders { get; private set; }
    
    public Collider Collider { get; private set; }

    public EnemyAggroHandler AggroHandler { get; private set; }
    public IMonsterVisibilityHandler VisibilityHandler { get; private set; }
    public ActorHealth Health { get; private set; }
    public ActorArmor Armor { get; private set; }

    public virtual EntityType EntityType => EntityType.Enemy;
    public INavigationAgent Navigation { get; set; }
    public FlinchComponent Flinch { get; set; }
    public MonsterAttackComponent Attack { get; set; }

    public virtual bool IsGrounded => Navigation?.IsGrounded ?? true;


    protected virtual void Awake()
    {
        if (Animator == null)
        {
            Animator = GetComponentInChildren<Animator>();
        }

        Attack = GetComponentInChildren<MonsterAttackComponent>();
        ActorEvents = GetComponentInChildren<IActorEvents>();
        AnimationMaterialHelper = GetComponentInChildren<AnimationMaterialHelper>();
        HitscanColliders = GetComponentsInChildren<IDamagedByHitscanProjectile>();
        AggroHandler = GetComponentInChildren<EnemyAggroHandler>();
        VisibilityHandler = GetComponentInChildren<IMonsterVisibilityHandler>();
        Health = GetComponentInChildren<ActorHealth>();
        Armor = GetComponentInChildren<ActorArmor>();
        Navigation = GetComponentInChildren<INavigationAgent>();
        Flinch = GetComponentInChildren<FlinchComponent>();
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy") && collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                continue;
            }

            if (collider is MeshCollider)
            {
                continue;
                //Debug.LogWarning($"More than one collider found for {gameObject.name}!");
            }
            Collider = collider;
        }
    }

}