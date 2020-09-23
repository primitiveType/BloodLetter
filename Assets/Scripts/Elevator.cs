using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Elevator : MonoBehaviour
{
    private AudioSource audiosource;

    [SerializeField] private float delay = 2f;

    [FormerlySerializedAs("rigidbody")] [SerializeField]
    private Transform elevator;

    private Rigidbody rb;

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
        rb = elevator.GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(elevator.position, .5f);
        Gizmos.DrawLine(StartTarget.transform.position, EndTarget.transform.position);
    }

    private IEnumerator Move(Transform target)
    {
        yield return new WaitForFixedUpdate();

        var start = elevator.transform.position;
        // Vector3 start = transform.position;
        var targetPosition = target.position;
        var distance = Vector3.Distance(start, targetPosition);
        float t = 0;
        var direction = (targetPosition - start).normalized;
        var targetVelocity = direction * speed;
        var approxTotalTime = distance / speed;
        Debug.Log(approxTotalTime);
        while (t < 1f)
        {
            yield return new WaitForFixedUpdate();
            var currentTarget = Vector3.Lerp(start, targetPosition, t);
            // rb.velocity = targetVelocity;
            rb.MovePosition(currentTarget);
            t += Time.fixedDeltaTime /approxTotalTime ;
        }

        //elevator.transform.position = targetPosition;
        rb.MovePosition(targetPosition);
        rb.velocity = Vector3.zero;
        
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