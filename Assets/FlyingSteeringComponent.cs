using System.Collections.Generic;
using UnityEngine;

public class FlyingSteeringComponent : MonoBehaviour
{
    private Transform myTransform;
    private FlyingNavigation Navigation;
    [SerializeField] private int NumRaysPerAxis = 5;
    [SerializeField] private float RaycastDistance = 5f;

    private void Start()
    {
        myTransform = transform;
        Navigation = GetComponent<FlyingNavigation>();
    }

    private void OnDrawGizmosSelected()
    {
        myTransform = transform;
        if (Navigation == null) Navigation = GetComponent<FlyingNavigation>();

        foreach (var ray in GetRays(Navigation.TargetPosition))
            Debug.DrawRay(ray.origin, ray.direction * RaycastDistance);
    }

    private IEnumerable<Ray> GetRays(Vector3 target)
    {
        var position = myTransform.position;
        var up = Vector3.up;
        var forward = (target - position).normalized;
        var right = Vector3.Cross(Vector3.up, forward);
        yield return new Ray(position, forward);
        yield return new Ray(position, up);
        yield return new Ray(position, right);
        yield return new Ray(position, -up);
        yield return new Ray(position, -right);

        for (var i = 0; i < NumRaysPerAxis; i++)
        {
            var increment = (i + 1f) / NumRaysPerAxis;
            yield return new Ray(position, Vector3.Lerp(forward, up, increment));
            yield return new Ray(position, Vector3.Lerp(forward, right, increment));
            yield return new Ray(position, Vector3.Lerp(forward, -up, increment));
            yield return new Ray(position, Vector3.Lerp(forward, -right, increment));
            yield return new Ray(position, Vector3.Lerp(right, up, increment));
            yield return new Ray(position, Vector3.Lerp(right, -up, increment));
            yield return new Ray(position, Vector3.Lerp(-right, up, increment));
            yield return new Ray(position, Vector3.Lerp(-right, -up, increment));
        }
    }

    public Vector3 GetAdjustedDirectionToTarget(Vector3 target)
    {
        var count = 0;
        var avgVector = Vector3.zero;
        foreach (var ray in GetRays(target))
            if (!Physics.Raycast(ray, RaycastDistance, LayerMask.GetMask("Default", "Interactable")))
            {
                avgVector += ray.direction;
                count++;
            }

        if (count == 0) return target; //errrrrr

        avgVector /= count;
        Debug.DrawRay(myTransform.position, avgVector, Color.magenta);
        return avgVector;
    }
}