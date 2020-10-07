using UnityEngine;

public class OnShotApplyForce : MonoBehaviour
{
    [SerializeField] private ActorRoot _root;
    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        _root.ActorEvents.OnShotEvent += ActorEventsOnOnShotEvent;
    }

    private void ActorEventsOnOnShotEvent(object sender, OnShotEventArgs args)
    {
        var force = args.HitNormal * args.ProjectileInfo.Force;
        rb.AddForce(force, ForceMode.Impulse);
    }

}