using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevator : Elevator
{
    [SerializeField] private InteractableKey Key;
    [SerializeField] private Transform MiddleTarget;

    protected override void Start()
    {
        base.Start();
        ThreeStageElevatorIdService.Instance.RegisterInteractable(this, Key);
    }


    private void MoveToTransform(Transform target)
    {
        if (MoveCR != null)
        {
            EnableAudio(false);
            StopCoroutine(MoveCR);
        }

        MoveCR = StartCoroutine(TriggerCr(target));
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

    public void MoveToStart()
    {
        MoveToTransform(StartTarget);
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

    protected override void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(ElevatorTransform.position, .5f);
        Gizmos.DrawLine(StartTarget.transform.position, MiddleTarget.transform.position);
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(MiddleTarget.transform.position, EndTarget.transform.position);
        Gizmos.color = color;
    }
}