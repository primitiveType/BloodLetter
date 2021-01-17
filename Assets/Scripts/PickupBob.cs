using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBob : MonoBehaviour
{
    private Transform myTransform;

    [SerializeField] private float bobDistance = .1f;
    [SerializeField] private float bobTime = 1f;
    private float halfBobTime => bobTime / 2f;

    private Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        startPosition = myTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var yPos = Mathf.PingPong(Time.time, bobTime) * bobDistance;
           
        myTransform.localPosition = startPosition + new Vector3(0, yPos, 0);
    }
}