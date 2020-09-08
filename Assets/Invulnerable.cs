using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    [SerializeField] private DamageType damageToIgnore;
    [SerializeField] private float duration = 30;
    private IPostProcessHandle ppHandle;

    private float timestamp;

    public DamageType DamageToIgnore
    {
        get => damageToIgnore;
        set => damageToIgnore = value;
    }

    public void Start()
    {
        timestamp = Time.time;
        if (DamageToIgnore.HasFlag(DamageType.Attack))
            ppHandle = PostProcessingManager.Instance.EnableInvulnEffect();
        else if (damageToIgnore.HasFlag(DamageType.Hazard))
            ppHandle = PostProcessingManager.Instance.EnableGasMaskEffect();
    }

    private void Update()
    {
        if (timestamp + duration < Time.time) Destroy(this);
    }

    private void OnDestroy()
    {
        ppHandle?.Dispose();
    }
}