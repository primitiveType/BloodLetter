using System.Collections;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private ActorEvents m_Events;
    private ActorEvents Events => m_Events;

    [SerializeField] private float delay;
    [SerializeField] private bool WaitUntilNotRendered;

    [SerializeField] private MeshRenderer m_Renderer;

    private void Awake()
    {
        Events.OnDeathEvent += OnDeath;
    }

    private void OnDeath(object sender, OnDeathEventArgs args)
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        if (WaitUntilNotRendered)
        {
            yield return new WaitUntil(()=>!m_Renderer.isVisible);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= OnDeath;
    }
}