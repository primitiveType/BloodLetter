using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Elevator : MonoBehaviour
{
    public float speed;

    [FormerlySerializedAs("BottomTarget")] [SerializeField]
    private Transform StartTarget;

    [FormerlySerializedAs("TopTarget")] [SerializeField]
    private Transform EndTarget;

    [FormerlySerializedAs("rigidbody")] [SerializeField]
    private Transform elevator;

    private AudioSource audiosource;

    [SerializeField] private float delay = 2f;
    [SerializeField] private bool returns = true;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
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
            // rigidbody.MovePosition(currentTarget);
            yield return null;
            t += (Time.deltaTime) / (distance / speed);
        }

        elevator.transform.position = targetPosition;

        // rigidbody.position = targetPosition;
    }

    private Coroutine MoveCR;

    public void Trigger()
    {
        if (MoveCR != null)
            StopCoroutine(MoveCR);

        MoveCR = StartCoroutine(TriggerCr());
    }

    private IEnumerator TriggerCr()
    {
        EnableAudio(true);

        Transform target = EndTarget;
        Transform returnTarget = StartTarget;

        yield return Move(target);

        if (returns)
        {
            EnableAudio(false);

            yield return new WaitForSeconds(delay);
            EnableAudio(true);

            yield return Move(returnTarget);
        }

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