using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private ActorEvents m_Events;
    private ActorEvents Events => m_Events;

    private void Awake()
    {
        Events.OnDeathEvent += OnDeath;
    }

    private void OnDeath(object sender, OnDeathEventArgs args)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= OnDeath;
    }
}