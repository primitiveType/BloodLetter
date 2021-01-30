using CodingEssentials;
using UnityEngine;

public class InstantaneousAoeSlow : InstantaneousAoe
{
    [SerializeField] private float SlowDuration = 2f;
    protected override void OnHit(Collider collider, Vector3 direction, Vector3 position)
    {
        var slow = collider.GetOrAddComponent<SlowMovement>();
        slow.AddDuration(SlowDuration);
    }
}