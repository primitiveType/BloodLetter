using UnityEngine;

public class DamagedByHitscanProjectile : MonoBehaviour, IDamagedByHitscanProjectile
{
    [SerializeField] private ActorRoot _actorRoot;
    private AnimationMaterialHelper MaterialHelper => ActorRoot.AnimationMaterialHelper;
    private IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot
    {
        get => _actorRoot;
        set => _actorRoot = value;
    }

    public bool IsHit(Vector2 textureCoord)
    {
        if (!enabled) return false;

        return MaterialHelper.QueryAlpha(textureCoord);
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public virtual void OnShot(Vector2 textureCoord, Vector3 worldPos, HitscanProjectileInfo projectileInfo, Vector3 hitNormal)
    {
        Events.OnShot(projectileInfo, worldPos, hitNormal);
    }

    public void 
        OnShot(HitscanProjectileInfo projectileInfo, Vector3 worldPos, Vector3 hitNormal)
    {
        Events.OnShot(projectileInfo, worldPos, hitNormal);
    }

    private void Start()
    {
        if (ActorRoot == null) ActorRoot = GetComponentInParent<ActorRoot>();
    }
}