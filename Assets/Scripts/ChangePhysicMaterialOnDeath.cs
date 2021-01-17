using UnityEngine;

public class ChangePhysicMaterialOnDeath : MonoBehaviour
{
    private Collider collider;

    [SerializeField] private PhysicMaterial material;
    private IActorEvents Events => ActorRoot.ActorEvents;

    private ActorRoot ActorRoot { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
        collider = GetComponent<Collider>();
        Events.OnDeathEvent += EventsOnOnDeathEvent;
    }

    private void EventsOnOnDeathEvent(object sender, OnDeathEventArgs args)
    {
        collider.material = material;
    }

    private void OnDestroy()
    {
        Events.OnDeathEvent -= EventsOnOnDeathEvent;
    }
}