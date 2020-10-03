using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepEvents : MonoBehaviour
{
    private ActorRoot ActorRoot;
    private float distanceBetweenFootPrints = 3f;
    private Vector3 lastPosition;
    private void Start()
    {
        ActorRoot = GetComponentInParent<ActorRoot>();
    }

    private float timer;
    private void Update()
    {
        var position = transform.position;

        if (Vector3.Distance(lastPosition, position) > distanceBetweenFootPrints)
        {
            ActorRoot.ActorEvents.OnStep(lastPosition, position);
            lastPosition = position;
        }
    }
}
