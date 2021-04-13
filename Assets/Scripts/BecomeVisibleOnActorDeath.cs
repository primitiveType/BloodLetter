using System.Collections.Generic;
using UnityEngine;

public class BecomeVisibleOnActorDeath : MonoBehaviour
{
    public List<ActorRoot> Actors;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ActorRoot actor in Actors)
        {
            actor.ActorEvents.OnDeathEvent += ActorOnOnDeathEvent;
        }

        SetVisibility();
    }

    private void OnDestroy()
    {
        foreach (ActorRoot actor in Actors)
        {
            actor.ActorEvents.OnDeathEvent -= ActorOnOnDeathEvent;
        }
    }

    private void ActorOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        SetVisibility();
    }

    private void SetVisibility()
    {
        foreach (ActorRoot actor in Actors)
        {
            if (actor.Health.IsAlive)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        gameObject.SetActive(true);
    }
}