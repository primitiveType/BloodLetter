using UnityEngine;

public class TriggerOnShot : MonoBehaviour
{
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    [SerializeField] private Animator Animator;
    private IActorEvents Events;

    private void Start()
    {
        Events = GetComponent<IActorEvents>();
        Events.OnShotEvent += EventsOnOnShotEvent;
    }

    private void EventsOnOnShotEvent(object sender, OnShotEventArgs args)
    {
        Animator.SetTrigger(Trigger);
    }

    private void OnDestroy()
    {
        Events.OnShotEvent -= EventsOnOnShotEvent;
    }
}