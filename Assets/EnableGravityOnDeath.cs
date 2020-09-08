using UnityEngine;

public class EnableGravityOnDeath : MonoBehaviour
{
    private Rigidbody rb;
    private IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        rb = GetComponent<Rigidbody>();
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}