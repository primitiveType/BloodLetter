﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevator : MonoBehaviour
{
    public float speed;

    [SerializeField] private Transform StartTarget;

    [SerializeField] private Transform EndTarget;
    [SerializeField] private Transform MiddleTarget;

    [FormerlySerializedAs("rigidbody")] [SerializeField]
    private Transform elevator;

    private AudioSource audiosource;

    [SerializeField] private float predelay = 0f;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(StartTarget.transform.position, EndTarget.transform.position);
    }

    private IEnumerator Move(Transform target)
    {
        Vector3 start = elevator.transform.position;
        // Vector3 start = transform.position;
        var targetPosition = target.position;
        float distance = Vector3.Distance(start, targetPosition);
        float t = 0;

        yield return null;
        while (t < 1f)
        {
            var currentTarget = Vector3.Lerp(start, targetPosition, t);
            elevator.transform.position = currentTarget;
            yield return null;
            t += (Time.deltaTime) / (distance / speed);
        }

        elevator.transform.position = targetPosition;
    }

    private Coroutine MoveCR;

    private void MoveToTransform(Transform target)
    {
        if (MoveCR != null)
        {
            EnableAudio(false);
            StopCoroutine(MoveCR);
        }

        MoveCR = StartCoroutine(TriggerCr(target));
    }

    public void MoveToStart()
    {
        MoveToTransform(StartTarget);
    }
    
    public void MoveTo(ElevatorState state)
    {
        switch (state)
        {
            case ElevatorState.Start:
                MoveToStart();
                break;
            case ElevatorState.Middle:
                MoveToMiddle();
                break;
            case ElevatorState.End:
                MoveToEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }


    public void MoveToMiddle()
    {
        MoveToTransform(MiddleTarget);
    }

    public void MoveToEnd()
    {
        MoveToTransform(EndTarget);
    }

    private IEnumerator TriggerCr(Transform target)
    {
        yield return new WaitForSeconds(predelay);
        EnableAudio(true);


        yield return Move(target);

        EnableAudio(false);
    }

    private void EnableAudio(bool value)
    {
        if (audiosource)
        {
            audiosource.enabled = value;
        }
    }
}