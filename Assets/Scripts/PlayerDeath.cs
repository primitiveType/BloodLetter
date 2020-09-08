using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerEvents Events;

    [SerializeField] private MonoBehaviour ToDisable;

    // Start is called before the first frame update
    private void Start()
    {
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        ToDisable.enabled = false;
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }
}