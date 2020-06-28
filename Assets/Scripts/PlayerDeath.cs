using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private ActorEvents Events;
    [SerializeField] private MonoBehaviour ToDisable;
    // Start is called before the first frame update
    void Start()
    {
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        ToDisable.enabled = (false);
    }

    void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }
}
