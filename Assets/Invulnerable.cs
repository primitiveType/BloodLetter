
using UnityEngine;

public class Invulnerable : MonoBehaviour
{
    [SerializeField] private DamageType damageToIgnore;

    public DamageType DamageToIgnore
    {
        get => damageToIgnore;
        set => damageToIgnore = value;
    }

    private float timestamp;
    [SerializeField] private float duration = 30;
    private IPostProcessHandle ppHandle;

    public void Start()
    {
        timestamp = Time.time;
        if (DamageToIgnore.HasFlag(DamageType.Attack))
        {
            ppHandle = PostProcessingManager.Instance.EnableInvulnEffect();
        }
        else if (damageToIgnore.HasFlag(DamageType.Hazard))
        {
            ppHandle = PostProcessingManager.Instance.EnableGasMaskEffect();
        }
    }

    private void Update()
    {
        if (timestamp + duration < Time.time)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        ppHandle?.Dispose();
    }
}