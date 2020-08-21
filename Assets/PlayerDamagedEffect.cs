using System.Collections;
using UnityEngine;

public class PlayerDamagedEffect : MonoBehaviour
{
    public PlayerRoot PlayerRoot { get; private set; }
    private IPostProcessHandle ppHandle;
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
    }

    private void OnHealthChangedEvent(object sender, OnHealthChangedEventArgs args)
    {
        ppHandle?.SetWeight(ratio);
        if (FlashCoroutine != null)
        {
            StopCoroutine(FlashCoroutine);
            
        }

        FlashCoroutine = StartCoroutine(FlashCr());
    }

    private Coroutine FlashCoroutine;

    private IEnumerator FlashCr()
    {
        ppHandle?.SetWeight(1);
        float duration = .5f;
        float time = Time.time;
        float t = 0;
        float dest = ratio;
        float start = Mathf.Clamp01( dest + .5f);
        while (t <= duration)
        {
            ppHandle?.SetWeight(Mathf.Lerp(start, dest, t / duration));
            t += Time.deltaTime;
            yield return null;
        }
    }
}