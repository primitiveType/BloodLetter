using System.Collections;
using UnityEngine;

public class PlayerDamagedEffect : MonoBehaviour
{
    private Coroutine FlashCoroutine;
    private IPostProcessHandle ppHandle;
    public PlayerRoot PlayerRoot { get; private set; }
    private float ratio => (100f - Toolbox.Instance.PlayerRoot.Health.Health) / 100f;

    private void Start()
    {
        PlayerRoot = GetComponentInParent<PlayerRoot>();
        ppHandle = PostProcessingManager.Instance.EnableDamagedEffect(0);
        Toolbox.Instance.PlayerEvents.OnHealthChangedEvent += OnHealthChangedEvent;
    }

    private void OnDestroy()
    {
        Toolbox.Instance.PlayerEvents.OnHealthChangedEvent -= OnHealthChangedEvent;
        ppHandle?.Dispose();
    }

    private void OnHealthChangedEvent(object sender, OnHealthChangedEventArgs args)
    {
        ppHandle?.SetWeight(ratio);
        if (args.IsHealing) return;

        if (FlashCoroutine != null) StopCoroutine(FlashCoroutine);

        FlashCoroutine = StartCoroutine(FlashCr());
    }

    private IEnumerator FlashCr()
    {
        ppHandle?.SetWeight(1);
        var duration = .5f;
        var time = Time.time;
        float t = 0;
        var dest = ratio;
        var start = Mathf.Clamp01(dest + .5f);
        while (t <= duration)
        {
            ppHandle?.SetWeight(Mathf.Lerp(start, dest, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
    }
    
    
}