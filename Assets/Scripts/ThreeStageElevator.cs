using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ThreeStageElevator : MonoBehaviour
{
    private AudioSource audiosource;

    [FormerlySerializedAs("rigidbody")] [SerializeField]
    private Transform elevator;

    [SerializeField] private Transform EndTarget;
    [SerializeField] private InteractableKey Key;
    [SerializeField] private Transform MiddleTarget;

    private Coroutine MoveCR;

    [SerializeField] private float predelay;

    public float speed;

    [SerializeField] private Transform StartTarget;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        ThreeStageElevatorIdService.Instance.RegisterInteractable(this, Key);
    }

    private void OnDrawGizmos()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(elevator.position, .5f);
        Gizmos.DrawLine(StartTarget.transform.position, MiddleTarget.transform.position);
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(MiddleTarget.transform.position, EndTarget.transform.position);
        Gizmos.color = Gizmos.color;
    }

    private IEnumerator Move(Transform target)
    {
        var start = elevator.transform.position;
        // Vector3 start = transform.position;
        var targetPosition = target.position;
        var distance = Vector3.Distance(start, targetPosition);
        float t = 0;

        yield return new WaitForFixedUpdate();
        while (t < 1f)
        {
            var currentTarget = Vector3.Lerp(start, targetPosition, t);
            elevator.transform.position = currentTarget;
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime / (distance / speed);
        }

        elevator.transform.position = targetPosition;
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
            if (value && !audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
    }
}