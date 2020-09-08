using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Elevator : MonoBehaviour
{
    private AudioSource audiosource;

    [SerializeField] private float delay = 2f;

    [FormerlySerializedAs("rigidbody")] [SerializeField]
    private Transform elevator;

    [FormerlySerializedAs("TopTarget")] [SerializeField]
    private Transform EndTarget;

    private Coroutine MoveCR;
    [SerializeField] private float predelay;
    [SerializeField] private bool returns = true;
    public float speed;

    [FormerlySerializedAs("BottomTarget")] [SerializeField]
    private Transform StartTarget;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(elevator.position, .5f);
        Gizmos.DrawLine(StartTarget.transform.position, EndTarget.transform.position);
    }

    private IEnumerator Move(Transform target)
    {
        var start = elevator.transform.position;
        // Vector3 start = transform.position;
        var targetPosition = target.position;
        var distance = Vector3.Distance(start, targetPosition);
        float t = 0;

        yield return null;
        while (t < 1f)
        {
            var currentTarget = Vector3.Lerp(start, targetPosition, t);
            elevator.transform.position = currentTarget;
            // rigidbody.MovePosition(currentTarget);
            yield return null;
            t += Time.deltaTime / (distance / speed);
        }

        elevator.transform.position = targetPosition;

        // rigidbody.position = targetPosition;
    }

    public void Trigger()
    {
        if (MoveCR != null)
            StopCoroutine(MoveCR);

        MoveCR = StartCoroutine(TriggerCr());
    }

    private IEnumerator TriggerCr()
    {
        yield return new WaitForSeconds(predelay);
        EnableAudio(true);

        var target = EndTarget;
        var returnTarget = StartTarget;

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
            if (value && !audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
    }
}